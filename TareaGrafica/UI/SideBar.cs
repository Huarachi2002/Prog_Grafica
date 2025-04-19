using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TareaGrafica.UI
{
    internal class SideBar
    {
        private Escenario escenario;
        private string selectedObjectKey = null;
        private string selectedPartKey = null;

        private Vector3 translationValue = new Vector3(0, 0, 0);
        private Vector3 rotationValue = new Vector3(0, 0, 0);
        private Vector3 scaleValue = new Vector3(1, 1, 1);

        private enum SelectionType
        {
            Escenario,
            Objeto,
            Parte
        }

        private SelectionType currentSelection = SelectionType.Escenario;

        public SideBar (Escenario escenario)
        {
            this.escenario = escenario;
        }

        public void Render()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X - 300, 0));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, ImGui.GetIO().DisplaySize.Y));

            ImGui.Begin("Control de Transformaciones", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);

            // Selector de nivel de transformación
            if (ImGui.RadioButton("Escenario", currentSelection == SelectionType.Escenario))
            {
                currentSelection = SelectionType.Escenario;
                selectedObjectKey = null;
                selectedPartKey = null;

                // Actualizar valores mostrados según el escenario
                UpdateValuesFromEscenario();
            }

            ImGui.SameLine();

            if(ImGui.RadioButton("Objeto", currentSelection == SelectionType.Objeto))
            {
                currentSelection = SelectionType.Objeto;
                selectedPartKey = null;

                // Si hay un objeto seleccionado, actualizar los valores mostrados
                if (selectedObjectKey != null && escenario.listaDeObjetos.ContainsKey(selectedObjectKey))
                {
                    UpdateValuesFromObject(escenario.listaDeObjetos[selectedObjectKey]);
                }
            }

            ImGui.SameLine();

            if (ImGui.RadioButton("Parte", currentSelection == SelectionType.Parte))
            {
                currentSelection = SelectionType.Parte;

                // Asegurarse que hay un objeto y una parte seleccionada
                if (selectedObjectKey != null && selectedPartKey != null)
                {
                    var objeto = escenario.listaDeObjetos[selectedObjectKey];
                    if (objeto.listaPartes.ContainsKey(selectedPartKey))
                    {
                        UpdateValuesFromParte(objeto.listaPartes[selectedPartKey]);
                    }
                }
            }

            ImGui.Separator();

            switch (currentSelection)
            {
                case SelectionType.Escenario:
                    RenderEscenarioControls();
                    break;

                case SelectionType.Objeto:
                    RenderObjectSelector();
                    if (selectedObjectKey != null)
                    {
                        RenderObjetoControls();
                    }
                    break;

                case SelectionType.Parte:
                    RenderObjectSelector();
                    if (selectedObjectKey != null)
                    {
                        RenderParteSelector();
                        if (selectedPartKey != null)
                        {
                            RenderParteControls();
                        }
                    }
                    break;
            }

            ImGui.End();
        }

        private void RenderEscenarioControls()
        {
            ImGui.Text("Transformaciones del Escenario");

            // Controles de traslación
            if (ImGui.DragFloat3("Traslación", ref translationValue, 0.1f))
            {
                escenario.Trasladar(translationValue.X, translationValue.Y, translationValue.Z);
            }

            // Controles de rotación
            if (ImGui.DragFloat3("Rotación", ref rotationValue, 1.0f))
            {
                escenario.Rotar(rotationValue.X, rotationValue.Y, rotationValue.Z);
            }

            // Controles de escala
            if (ImGui.DragFloat3("Escala", ref scaleValue, 0.1f, 0.1f, 10.0f))
            {
                escenario.Escalar(scaleValue.X, scaleValue.Y, scaleValue.Z);
            }
        }

        private void RenderObjectSelector()
        {
            if (ImGui.BeginCombo("Objeto", selectedObjectKey ?? "Seleccionar objeto"))
            {
                foreach (var key in escenario.listaDeObjetos.Keys)
                {
                    bool isSelected = key == selectedObjectKey;
                    if (ImGui.Selectable(key, isSelected))
                    {
                        selectedObjectKey = key;

                        // Actualizar valores según el objeto seleccionado
                        if (currentSelection == SelectionType.Objeto)
                        {
                            UpdateValuesFromObject(escenario.listaDeObjetos[key]);
                        }

                        // Resetear selección de parte
                        if (currentSelection == SelectionType.Parte)
                        {
                            selectedPartKey = null;
                        }
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }

        private void RenderObjetoControls()
        {
            var objeto = escenario.listaDeObjetos[selectedObjectKey];
            ImGui.Text($"Transformaciones para objeto: {selectedObjectKey}");

            // Control para el centro de masa
            Vector3 centroDeMasa = new Vector3(objeto.centroDeMasa.X, objeto.centroDeMasa.Y, objeto.centroDeMasa.Z);
            if (ImGui.DragFloat3("Centro de masa", ref centroDeMasa, 0.1f))
            {
                objeto.SetCentroDeMasa(new Punto(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z));
            }

            // Controles de rotación
            if (ImGui.DragFloat3("Rotación", ref rotationValue, 1.0f))
            {
                objeto.Rotar(rotationValue.X - objeto.rotacionX,
                           rotationValue.Y - objeto.rotacionY,
                           rotationValue.Z - objeto.rotacionZ);
            }

            // Controles de escala
            if (ImGui.DragFloat3("Escala", ref scaleValue, 0.1f, 0.1f, 10.0f))
            {
                objeto.Escalar(scaleValue.X, scaleValue.Y, scaleValue.Z);
            }
        }

        private void RenderParteSelector()
        {
            if (selectedObjectKey == null) return;

            var objeto = escenario.listaDeObjetos[selectedObjectKey];

            if (ImGui.BeginCombo("Parte", selectedPartKey ?? "Seleccionar parte"))
            {
                foreach (var key in objeto.listaPartes.Keys)
                {
                    bool isSelected = key == selectedPartKey;
                    if (ImGui.Selectable(key, isSelected))
                    {
                        selectedPartKey = key;

                        // Actualizar valores según la parte seleccionada
                        UpdateValuesFromParte(objeto.listaPartes[key]);
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }

        private void RenderParteControls()
        {
            if (selectedObjectKey == null || selectedPartKey == null) return;

            var objeto = escenario.listaDeObjetos[selectedObjectKey];
            var parte = objeto.listaPartes[selectedPartKey];

            ImGui.Text($"Transformaciones para parte: {selectedPartKey}");

            // Control para el centro de masa
            Vector3 centroMasa = new Vector3(parte.centroMasa.X, parte.centroMasa.Y, parte.centroMasa.Z);
            if (ImGui.DragFloat3("Centro de masa", ref centroMasa, 0.1f))
            {
                parte.SetCentroMasa(new Punto(centroMasa.X, centroMasa.Y, centroMasa.Z));
            }

            // Controles de rotación
            if (ImGui.DragFloat3("Rotación", ref rotationValue, 1.0f))
            {
                parte.Rotar(rotationValue.X - parte.rotacionX,
                          rotationValue.Y - parte.rotacionY,
                          rotationValue.Z - parte.rotacionZ);
            }

            // Si hay un método de escala para Parte (que no está en tu código compartido),
            // aquí iría la implementación similar
        }

        private void UpdateValuesFromEscenario()
        {
            translationValue = new Vector3(escenario.traslacionX, escenario.traslacionY, escenario.traslacionZ);
            rotationValue = new Vector3(escenario.rotacionX, escenario.rotacionY, escenario.rotacionZ);
            scaleValue = new Vector3(escenario.escalaX, escenario.escalaY, escenario.escalaZ);
        }

        private void UpdateValuesFromObject(Objeto objeto)
        {
            rotationValue = new Vector3(objeto.rotacionX, objeto.rotacionY, objeto.rotacionZ);
            scaleValue = new Vector3(objeto.escalaX, objeto.escalaY, objeto.escalaZ);
        }

        private void UpdateValuesFromParte(Parte parte)
        {
            rotationValue = new Vector3(parte.rotacionX, parte.rotacionY, parte.rotacionZ);
            // Si parte tiene escala, actualizarla aquí
        }
    }
}
