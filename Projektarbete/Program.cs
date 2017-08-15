using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektarbete
{

    class Program
    {
        static List<int> membersPerGroup = new List<int>();
        static List<string> InputNames() //Frågar efter gruppmedlemmar
        {
            Console.Write("Vill du läsa in gruppmedlemmarna från en fil? (Ja/Nej): ");
            string input = Console.ReadLine();
            while (!input.Equals("Ja", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("Nej", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Write("Felaktigt svar, ange ja eller nej. ");
                input = Console.ReadLine();
            }

            if (input.Equals("ja", StringComparison.InvariantCultureIgnoreCase))
            {
                string[] text = System.IO.File.ReadAllLines("klassnamn.txt");
                List<string> namesList = new List<string>();
                foreach (string name in text)
                {
                    namesList.Add(name);
                }
                return namesList;
            }
            else
                Console.Write("Ange gruppmedlemmar här, separera namn med komma (exempel Lisa,Pelle,Lotta): ");
            string names = Console.ReadLine();
            return NameList(names);
        }

        static List<string> NameList(string names)  //Gör om vår string till en lista
        {
            List<string> nameArray = names.Split(',').ToList<string>();
            return nameArray;

        }

        static List<string> CreateGroups(List<string> members) //Skapa grupper
        {
            int groupNumbers = AskForNumbersOfGroups(members);
            while (members.Count < groupNumbers)
            {
                Console.WriteLine("Antal grupper var större än antalet medlemmar.");
                groupNumbers = AskForNumbersOfGroups(members);
            }
            List<string> groups = new List<string>();

            for (int i = 0; i < groupNumbers; i++)
            {
                Console.Write("Ange namn för grupp nummer " + (i + 1) + ": ");
                string groupName = Console.ReadLine();
                groups.Add(groupName);
            }
            return groups;
        }

        static bool AskForCustomMadeGroups(List<string> membersList, int numberOfGroups)
        {
            Console.WriteLine("Vill du ange specifikt antal medlemmar per grupp?: ");
            string input = Console.ReadLine();
            while (!input.Equals("Ja", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("Nej", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Write("Felaktigt svar, ange ja eller nej. " );
                input = Console.ReadLine();
            }
            bool b = false;
            if (input.Equals("ja", StringComparison.InvariantCultureIgnoreCase))
            {
                while (!b)
                {
                    b = DecideNumberOfMembers(membersList, numberOfGroups);
                }
                return true;
            }
            return false;
        }

        static bool DecideNumberOfMembers(List<string> membersList, int numberOfGroups)
        {
            int sumOfGroupsMembers = 0;
            List<int> numbersPerGroup = new List<int>();
            for (int i = 0; i < numberOfGroups; i++)
            {
                Console.WriteLine("Antal medlemmar kvar att fördela är " + (membersList.Count - sumOfGroupsMembers) + " på " + (numberOfGroups - (i)) + " grupper");
                Console.Write("Ange antal för grupp nummer " + (i + 1) + ": ");
                string inputInt = Console.ReadLine();
                while (!CheckIfInputInt(inputInt))
                {
                    Console.Write("Du angav ej ett tal, ange tal på nytt: ");
                    inputInt = Console.ReadLine();
                }
                sumOfGroupsMembers = sumOfGroupsMembers + int.Parse(inputInt);
                if (sumOfGroupsMembers > membersList.Count - (numberOfGroups - (i+1)))
                {
                    Console.WriteLine("Antalet går inte upp");
                    return false;
                }
                numbersPerGroup.Add(int.Parse(inputInt));
            }
            if (membersList.Count - sumOfGroupsMembers == 0)
            {
                membersPerGroup = numbersPerGroup;
                return true;
            }
            return false;
        }

        static int AskForNumbersOfGroups(List<string> members)
        {
            Console.Write("Ange antal grupper: ");
            string input = Console.ReadLine();
            while (!CheckIfInputInt(input))
            {
                Console.Write("Du angav ej ett tal, ange antal grupper på nytt: ");
                input = Console.ReadLine();
            }
            int groupNumbers = int.Parse(input);
            return groupNumbers;
        }

        static bool CheckIfInputInt(string input)
        {
            int number;
            bool b = int.TryParse(input, out number);
            return b;
        }

        static List<List<string>> AddMembers(List<string> members, List<string> groups)
        {
            List<List<string>> groupsWithMembers = new List<List<string>>();
            List<string> shuffledMembers = Shuffle(members);
            int minimumMembersPerGroup = (shuffledMembers.Count / groups.Count);
            int rest = shuffledMembers.Count % groups.Count;

            for (int i = 0; i < groups.Count; i++)
            {
                List<string> membersList = new List<string>();
                membersList.Add(groups[i]);
                for (int t = 0; t < minimumMembersPerGroup; t++)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                if (rest > i)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                groupsWithMembers.Add(membersList);
            }
            return groupsWithMembers;
        }

        static List<List<string>> AddMembersSpecificNumbers(List<string> members, List<string> groups, List<int> groupNumbers)
        {
            List<List<string>> groupsWithMembers = new List<List<string>>();
            List<string> shuffledMembers = Shuffle(members);
            for (int i = 0; i < groups.Count; i++)
            {
                int number = groupNumbers[i];
                List<string> membersList = new List<string>();
                membersList.Add(groups[i]);
                for (int t = 0; t < number; t++)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                groupsWithMembers.Add(membersList);
            }
            return groupsWithMembers;
        }

        static List<string> Shuffle(List<string> array) // Blanda lista
        {
            Random random = new Random();
            int n = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                int r = i + random.Next(n - i);
                string t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
            return array;
        }

        static void PrintGroups(List<List<string>> lists)
        {
            Console.WriteLine();
            foreach (List<string> list in lists)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(list[i].Trim());
                        Console.ResetColor();
                    }
                    else if (i == 1)
                    {

                        Console.WriteLine(list[i].Trim() + " <--- Gruppledare");
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.WriteLine(list[i].Trim());
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(list[i].Trim());
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            List<string> members = InputNames();
            List<string> groups = CreateGroups(members);
            if (AskForCustomMadeGroups(members, groups.Count))
            {
                List<List<string>> groupsWithMembers = AddMembersSpecificNumbers(members, groups, membersPerGroup);
                PrintGroups(groupsWithMembers);
            }
            else
            {
                List<List<string>> groupsWithMembers = AddMembers(members, groups);
                PrintGroups(groupsWithMembers);
            }   
        }
    }
}