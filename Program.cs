using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Reflection.Metadata;

namespace project2;

class Program
{
    static List<User> users = new List<User>();
    const string filePath = "data.json"; 
    static string? username, password;
    static bool logStatus = false;
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
        #pragma warning disable CS8601 // Possible null reference assignment.
            users = JsonSerializer.Deserialize<List<User>>(jsonData);
        #pragma warning restore CS8601 // Possible null reference assignment.
        }
        
        while(true){
            HomePage();

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if(keyInfo.KeyChar == '1'){
                logStatus = Login();
                while(logStatus){
                    Menu();
                }
            }else if(keyInfo.KeyChar == '2'){
                CreateAccount();
                SaveData();
            }else if(keyInfo.KeyChar == '3'){
                Console.Clear();
                Console.WriteLine("               DEV               ");
                Console.WriteLine("       Richard Miquel Laia       ");
                Console.ReadLine();
            }
            else if(keyInfo.KeyChar == '4'){
                Console.Clear();
                Console.WriteLine("Thank You... Have a Nice Day!!");
                Environment.Exit(0);
            }
        }  
    }

    static void HomePage(){
        Console.Clear();
        printLine(45);
        Console.WriteLine();
        Console.WriteLine("1 | LOGIN                         SIGN UP | 2\n\n\n\n");
        Console.WriteLine("3 | ABOUT                            EXIT | 4");
        Console.WriteLine();
        printLine(45);
    }

    static void Menu(){
        Console.Clear();
        printLine(45);
        Console.WriteLine();
        Console.WriteLine("1 | BALANCE                      TRANSFER | 2\n\n");
        Console.WriteLine("3 | WITHDRAW                       TOP UP | 4\n\n");
        Console.WriteLine("5 | LOG OUT ");
        Console.WriteLine();
        printLine(45);

        User? user = users.Find(u => u.Username == username);

        ConsoleKeyInfo key = Console.ReadKey();
        switch(key.KeyChar){
            case '1':
                Console.Clear();
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                Console.Write($"\n\n        Balance: {user.Balance} USD\n");
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
                Console.Write("        Press ENTER.");
                Console.ReadLine();
                break;
            case '2':
                Console.Clear();
                Console.Write("   Transfer to: ");
                string? usr = Console.ReadLine();
                Console.Write("\n        Amount: ");
                int transferAmount = Convert.ToInt32(Console.ReadLine());
                Console.Write("\n        Amount: ");
                int pin = Convert.ToInt32(Console.ReadLine());
                checkPin(username, pin);

                user.Balance -= transferAmount;
                Transfer(usr, transferAmount);
                SaveData();
                Console.WriteLine("\n        Done...");
                Console.ReadLine();
                break;
            case '3':
                Console.Clear();
                Console.Write("\n        Amount: ");
                int withdrawAmount = Convert.ToInt32(Console.ReadLine());
                user.Balance -= withdrawAmount;
                SaveData();
                Console.WriteLine("\n        Done...");
                Console.ReadLine();
                break;
            case '4':
                Console.Clear();
                Console.Write("        Amount: ");
                int topupAmount = Convert.ToInt32(Console.ReadLine());
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                user.Balance += topupAmount;
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
                Console.WriteLine("\n        Verification...");
                Thread.Sleep(2000);
                Console.WriteLine("        Processing...");
                Thread.Sleep(2000);
                SaveData();
                Console.Write("        Done...");   
                Console.ReadLine();
                break;
            case '5':
                username = null;
                password = null;
                logStatus = false;
                break;
            default :
                Console.WriteLine("Input not Valid.");
                Menu();
                break;
        }
    }

    static bool Login()
    {
        Console.Clear();
        Console.WriteLine("\n              Login");

        Console.Write("          Username: ");
        username = Console.ReadLine();

        Console.Write("          Password: ");
        password = Console.ReadLine();

        User? user = users.Find(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            return true;
        }
        else
        {
            Console.WriteLine("Failed to Login. Incorrect Username or Password");
            return false;
        }
    }

    static void Transfer(string usrnm, int amount){
        User? user = users.Find(u => u.Username == usrnm);
        user.Balance += amount;
    }

    static void CreateAccount()
    {
        Console.Clear();
        Console.WriteLine("\n            Create Account");

        Console.Write("          Username: ");
        string? username = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("\n            Create Account");
        Console.Write("          Password: ");
        string? password = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("\n            Create Account");
        Console.Write("               PIN: ");
        int pin;
        while (!int.TryParse(Console.ReadLine(), out pin))
        {
            Console.Write("PIN must be number.");
        }

        User newUser = new User
        {
            Username = username,
            Password = password,
            Pin = pin,
            Balance = 0
        };

        users.Add(newUser);

        Console.WriteLine("Create Succes.");
    }

    static void SaveData()
    {
        // Menyimpan data pengguna ke file saat program keluar
        string jsonData = JsonSerializer.Serialize(users);
        File.WriteAllText(filePath, jsonData);
    }

    static void printLine(int length){
        for(int i = 0;i < length;i++){
            Console.Write('-');
        }
        Console.WriteLine();
    }

    static bool checkPin(string username, int pin){
        User? user = users.Find(u => u.Username==username  && u.Pin == pin);
        if(user != null){
            return true;
        }else{
            return false;
        }
    }
}

class User{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int Pin { get; set; }
    public int Balance{ get; set; }
}