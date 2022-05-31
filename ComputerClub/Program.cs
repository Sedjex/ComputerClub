using System;
using System.Collections.Generic;

namespace ComputerClub
{
    class Program
    {
        static void Main(string[] args)
        {
            ComputerClub computerClub = new ComputerClub(8);
            computerClub.Work();
            //Console.ReadKey();
        }
    }

    class ComputerClub
    {
        private int _money = 0;
        private List<Computer> _computers = new List<Computer>();
        private Queue<User> _users = new Queue<User>();

        public ComputerClub(int computerCount)
        {
            Random rand = new Random();
            for (int i = 0; i < computerCount; i++)
            {
                _computers.Add(new Computer(rand.Next(5, 30)));
            }

            CreateNewUser(25);
        }

        private void CreateNewUser(int count)
        {
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                _users.Enqueue(new User(rand.Next(100, 250), rand));
            }
        }

        public void Work()
        {
            while (_users.Count > 0)
            {
                Console.WriteLine("У компʼютерного клуба зараз " + _money + " чекаєм клієнтів..");

                User user = _users.Dequeue();
                Console.WriteLine("У черзі клієнт, хоче придбати " + user.DesiredMinutes + " хвилин");
                Console.WriteLine("\n Список компʼютерів: ");
                ShowAllComputers();

                Console.WriteLine("\n Пропонуємо ПК під номером: ");
                int computerNumber = Convert.ToInt32(Console.ReadLine());

                if (computerNumber >= 0 && computerNumber < _computers.Count)
                {
                    if (_computers[computerNumber].IsBusy)
                    {
                        Console.WriteLine("Ви запропонували вже зайнятий компʼютер. Клієнт пішов..");
                    }
                    else
                    {
                        if (user.CheckSolvency(_computers[computerNumber]))
                        {
                            Console.WriteLine("Клієнт оплатив і сів за компʼютер");
                            _money += user.ToPay();

                            _computers[computerNumber].TakeThePlace(user);
                        }
                        else
                        {
                            Console.WriteLine("У клієнта невистачило грошей і він пішов..");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Номер невірнийю Клієнт пішов..");
                }

                Console.WriteLine("Щоб перейти до нового клієнта натисніть будь-яку клавішу");
                Console.ReadKey();
                Console.Clear();
                SkipMinute();
            }
        }

        public void SkipMinute()
        {
            foreach (var comput in _computers)
            {
                comput.SkipMinute();
            }
        }

        private void ShowAllComputers()
        {
            for (int i = 0; i < _computers.Count; i++)
            {
                Console.WriteLine(i + " - ");
                _computers[i].ShovInfo();
            }
        }
    }

    class Computer
    {
        private User _user;
        private int _minutesLeft;

        public int PriceForMinutes { get; private set; }
        public bool IsBusy
        {
            get
            {
                return _minutesLeft > 0;
            }
        }

        public Computer(int priceForMinutes)
        {
            PriceForMinutes = priceForMinutes;
        }

        public void TakeThePlace(User user)
        {
            _user = user;
            _minutesLeft = _user.DesiredMinutes;
        }

        public void FreeThePlace()
        {
            _user = null;
        }

        public void SkipMinute()
        {
            _minutesLeft--;
        }

        public void ShovInfo()
        {
            if (IsBusy)
                Console.WriteLine("Компʼютер занят. Залишилось хвилин - " + _minutesLeft);
            else
                Console.WriteLine("Компʼютер вільний, ціна за хвилину - " + PriceForMinutes);
        }
    }

    class User
    {
        private int _money;
        private int _moneyToPay;
        public int DesiredMinutes { get; private set; }

        public User(int money, Random rand)
        {
            _money = money;
            DesiredMinutes = rand.Next(5, 30);
        }

        public bool CheckSolvency(Computer computer)
        {
            _moneyToPay = computer.PriceForMinutes * DesiredMinutes;
            if (_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }

        public int ToPay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
        }
    }
}

