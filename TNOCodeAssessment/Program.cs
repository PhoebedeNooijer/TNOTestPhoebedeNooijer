using System.Runtime.CompilerServices;
using System.Timers;

internal class Program
{
    static readonly double PKRatio = 0.8;
    static Random Random = new Random();
    static int SecondCounter = 0;
    static int EnemiesDetectedCounter = 0;
    static int TargetsHitCounter = 0; 

    static void Main(string[] args)
    {
        System.Timers.Timer myTimer = new System.Timers.Timer();
        myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
        myTimer.Interval = 1000;

        var path = @args[0];

        myTimer.Start();

        while (SecondCounter < 20)
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = new string(values[i].Where(c => c == "0" || c == "1").ToArray());
                    }

                    if (IsEnemy(values))
                    {
                        EnemiesDetectedCounter++;
                        if (Random.NextDouble() <= PKRatio)
                        {

                            TargetsHitCounter++;
                        }
                    }
                }
                reader.Close();
            }
        }
    }

    static bool IsEnemy(string[] list)
    {
        List<bool> isEvenList = new List<bool>();
        foreach (var item in list)
        {
            isEvenList.Add(Convert.ToInt64(item, 2)%2 == 0);
        }
        return isEvenList.Where(x => x == true).Count() < (isEvenList.Count / 2);
    }

    public static void DisplayTimeEvent(object source, ElapsedEventArgs e)
    {
        Console.WriteLine("Tick " + (SecondCounter + 1));
        if (EnemiesDetectedCounter == 0)
        {
            Console.WriteLine("No enemies detected"); 
        }
        else
        {
            Console.WriteLine("Enemies detected: " + EnemiesDetectedCounter);
            Console.WriteLine("Targets hit: " + TargetsHitCounter);
            Console.WriteLine("Targets missed: " + (EnemiesDetectedCounter - TargetsHitCounter)); 
        }
        Console.WriteLine(); 

        EnemiesDetectedCounter = 0;
        TargetsHitCounter = 0;

        SecondCounter++;
    }
}