namespace TareaGrafica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hola mundo");

            using (Game game = new(500, 500))
            {
                game.Run();
            }
        }
    }
}
