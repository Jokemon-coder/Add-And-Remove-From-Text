using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace classTestailua
{
    public class Human
    {
        //private string name;
        public string Name
        {
            get;
            set;
        }

        //private int age;
        public int Age
        {
            get;
            set;
        }

        //private string gender;
        public string Gender
        {
            get;
            set;
        }

        //private string id;

        public string Id
        {
            get;
            set;
        }

        public override string ToString()
        {
            //Format the variables into a readable string
            return string.Format("ID {0}: {1}, {2}-v, {3}", Id, Name, Age, Gender);
        }

        public static void GetSavedItems(bool specific, int specificNum)
        {
            int id = File.ReadAllLines("People.txt").Length;
            string[] data = File.ReadAllLines("People.txt");
            if (specific == false) //Get all saved items with a for loop, if bool specific is false
            {
                for (int i = 0; i < id; i++)
                {
                    Console.WriteLine(data[i]);

                }
            }
            else //Get a specific item if bool specific is set to true
            {
                Console.WriteLine(File.ReadLines("People.txt").Skip(specificNum).Take(1).First());
                /*while (i == specificNum)
                {
                  Console.WriteLine(File.ReadLines("People.txt").Skip(i).Take(1).First());
                  i++;     
                }*/
            }

        }

        public static void editName(string file, int line, string[] delimiters)
        {
            //Declare data and item here, as this actually makes them update in real time. 
            string[] data = File.ReadAllLines("People.txt");
            string item = data[line];

            //Splitting the selected item with the delimiters
            string[] selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);

            //Create a new string first, which adds the ID number plus the first element after (NAME)
            string firstHalfString = string.Join(": ", selectedItemContentArray[0], selectedItemContentArray[1] + ", ");
            
            //Then create a string for the original content, which is everything after ID and the first element (NAME)
            string secondHalfString = string.Join(", ", selectedItemContentArray[2], selectedItemContentArray[3]);
            
            //Make original content be both combined
            string originalContent = firstHalfString + secondHalfString;
            
            //editedContent will be the result of editGeneral()
            string editedContent = editGeneral(item, delimiters, firstHalfString, "name");

            //Read and replace the original content with the edited content and then write it into the file
            string text = File.ReadAllText(file);
            text = text.Replace(originalContent, editedContent);
            File.WriteAllText(file, text);

        }

        public static void editAge(string file, int line, string[] delimiters)
        {
            string[] data = File.ReadAllLines("People.txt");
            string item = data[line];

            string[] selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);

            string firstHalfString = string.Join(": ", selectedItemContentArray[0], selectedItemContentArray[1] + ", ");
            string secondHalfString = string.Join(", ", selectedItemContentArray[2], selectedItemContentArray[3]);
            
            string originalContent = firstHalfString + secondHalfString;
            string editedContent = firstHalfString + editGeneral(item, delimiters, secondHalfString, "age");

            string text = File.ReadAllText(file);
            text = text.Replace(originalContent, editedContent);
            File.WriteAllText(file, text);

        }

        public static void editGender(string file, int line, string[] delimiters)
        {
            string[] data = File.ReadAllLines("People.txt");
            string item = data[line];

            string[] selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);

            string firstHalfString = string.Join(": ", selectedItemContentArray[0], selectedItemContentArray[1] + ", ");
            string secondHalfString = string.Join(", ", selectedItemContentArray[2], selectedItemContentArray[3]);
            
            string originalContent = firstHalfString + secondHalfString;
            string editedContent = firstHalfString + editGeneral(item, delimiters, secondHalfString, "gender");

            string text = File.ReadAllText(file);
            text = text.Replace(originalContent, editedContent);
            File.WriteAllText(file, text);

        }


        public static void Delete(string file, int line)
        {
            //Replace the selected item with an empty string and then WriteAllLines clearing empty lines
            string[] data = File.ReadAllLines("People.txt");
            string item = data[line];
            string deletedString = "";

            string text;
            
            text = File.ReadAllText(file);
            text = text.Replace(item, deletedString);
            File.WriteAllText(file, text);
            File.WriteAllLines(file, File.ReadAllLines(file).Where(l => !string.IsNullOrWhiteSpace(l)));

            UpdateIdOnDelete();
            
        }

        private static string editGeneral(string item, string[] delimiters, string modified, string identifier)
        {
            //General editing function, that is called in all the editing sections
            //Comments are only going to be on the first with the identifier "name", since the rest follow the same logic with slight unique quirks to them
            string[] selectedItemContentArray;
            string editedContent;

            if(identifier == "name")
            {
                //Split the item into it's individual strings, in this case the one on index 1
               selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);
            editStart:
                Program.ClearAfterInput("Muokkaa nimeä: ");
                Console.WriteLine(selectedItemContentArray[1]);
                int editFailLimit = 0;
            editStartIfFail:
                string newContent = Console.ReadLine();
                if (Human.CheckIfDoesNotContainNumbers(newContent) != true) //Message and go back if the name contains numbers
                {
                    Console.WriteLine("Antamassasi nimessä on numeroita. Syötä vain kirjaimia.");
                    editFailLimit++;
                    if (Program.CheckFail(editFailLimit) == true)
                    {
                        goto editStart;
                    }
                    goto editStartIfFail;
                }
                //Replace the specific string in "modified" with the user input newContent
                modified = modified.Replace(selectedItemContentArray[1], newContent);
                
                //Set up the editedContent
                editedContent = string.Join(", ", selectedItemContentArray[2], selectedItemContentArray[3]);
                
                //Finalize editedContent by adding modified to the front of it
                editedContent = modified + editedContent;
                return editedContent;
            }

            if(identifier == "age")
            {
                selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);
            editStart:
                Program.ClearAfterInput("Muokkaa ikää: ");
                Console.WriteLine(selectedItemContentArray[2].Split("-v")[0]);
                int editFailLimit = 0;
            editStartIfFail:
                string newContent = Console.ReadLine();
                if (Human.CheckIfDoesNotContainNumbers(newContent) != false) //Message and go back if the age contains letters
                {
                    Console.WriteLine("Antamassasi iässä on kirjaimia.");
                    editFailLimit++;
                    if(Program.CheckFail(editFailLimit) == true)
                    {
                        goto editStart;
                    }
                    goto editStartIfFail;
                }
                else if (Convert.ToInt32(newContent) > 125) //If the age is more than 125, message and go back
                {
                    Console.WriteLine($"{newContent}? Tuota en kyllä usko. Anna hänen oikea ikä.");
                    editFailLimit++;
                    if (Program.CheckFail(editFailLimit) == true)
                    {
                        goto editStart;
                    }
                    goto editStartIfFail;
                }
                modified = modified.Replace(selectedItemContentArray[2], newContent + "-v");
                editedContent = modified;
                return editedContent;


            }
            if(identifier == "gender")
            {
                
                selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);
            editStart:
                Program.ClearAfterInput("Valitse mies painamalla 1 ja nainen painamalla 2.");
                Console.WriteLine("Muokkaa sukupuolta: ");
                Console.WriteLine(selectedItemContentArray[3]);
                int failLimit = 0;
            editStartIfFail:
                string newContent = Console.ReadLine();
                int num;
                if (!int.TryParse(newContent, out num)) //Message and go back if input can't be parsed to an int
                {
                    Console.WriteLine("Syötä vain numeroita.");
                    failLimit++;
                    if(Program.CheckFail(failLimit) == true)
                    {
                        goto editStart;
                    }
                    goto editStartIfFail;
                }

                if(Convert.ToInt32(newContent) == 1)
                {
                    newContent = "mies";
                    modified = modified.Replace(selectedItemContentArray[3], newContent);
                    editedContent = modified;
                    return editedContent;
                }
                else if (Convert.ToInt32(newContent) == 2)
                {
                    newContent = "nainen";
                    modified = modified.Replace(selectedItemContentArray[3], newContent);
                    editedContent = modified;
                    return editedContent;
                }
                else if (Convert.ToInt32(newContent) != 1 || Convert.ToInt32(newContent) != 2)
                {
                    Console.WriteLine("Syötä 1 tai 2.");
                    Console.WriteLine(num);
                    failLimit++;
                    if (Program.CheckFail(failLimit) == true)
                    {
                        goto editStart;
                    }
                    goto editStartIfFail;
                }


            }

            return "";
        }

        public static void CreateNewLineAndClear()
        {
            //Get the lines from the file.
            string[] fileContent = File.ReadAllLines("People.txt");
            int end = fileContent.Length;
            //If the file is 0 in length, meaning it has no lines other than the first, create an empty line. Otherwise clear that empty line.
            if (fileContent.Length == 0)
            {
                using (StreamWriter writer = new StreamWriter("People.txt", true))
                {
                    writer.WriteLine("");
                }
            }
            //If the length is not 0 and the first line is empty, clear that line
            else if (fileContent.Length != 0 && fileContent[0] == "")
            {
                File.WriteAllLines("People.txt", File.ReadAllLines("People.txt").Where(line => !string.IsNullOrWhiteSpace(line)));
            }
            //If the length is not 0, check all lines with for loop and clear them if they're empty
            else if (fileContent.Length != 0)
            {
                for (int i = 0; i > fileContent.Length - 1; i++)
                {
                    if (fileContent[i] == "")
                    {
                        File.WriteAllLines("People.txt", File.ReadAllLines("People.txt").Where(line => !string.IsNullOrWhiteSpace(line)));
                    }
                }
            }
        }

        public static void MakeId(int ids, Human person) //Create id based on the line at which the content is at. Empty lines will be cleared.
        {
            string[] fileContent = File.ReadAllLines("People.txt");
            for (int i = 0; i < ids; i++)
            {
                if (fileContent[i] == "")
                {

                    File.WriteAllLines("People.txt", File.ReadAllLines("People.txt").Where(line => !string.IsNullOrWhiteSpace(line)));
                    person.Id = Convert.ToString(File.ReadAllLines("People.txt").Length);
                }
                else
                {
                    person.Id = Convert.ToString(i);
                }
            }
        }

        private static void UpdateIdOnDelete()
        {
            //Get the file, delimiters for splitting and the file content in an array. 
            string file = "People.txt";
            string[] delimiters = { " ", ":" };
            string[] data = File.ReadAllLines(file);

            //Use a for loop to go through the data and split the item.
            for (int i = 0; i < data.Length; i++) 
            {
                string item = data[i];
                string[] selectedItemContentArray = item.Split(delimiters, StringSplitOptions.None);

                //Make id the 2nd element of the content array, that being the actual id number.
                int id = Convert.ToInt32(selectedItemContentArray[1]);
                if (i > id && i != 0) //If the item and it's position is larger than the id and the position is not 0, increase the id.
                {
                    id++;
                }
                else if (i < id) //If the item position is larger than id, just decreased the id. 
                {
                    id--;
                }
                //Then replace the old content with the increased or decreased id and write it to the file.
                item = item.Replace(selectedItemContentArray[1], Convert.ToString(id));
                string text = File.ReadAllText(file);
                text = text.Replace(data[i], item);
                File.WriteAllText(file, text);

            }
        }

        public static bool CheckIfDoesNotContainNumbers(string text)
        {
            //If the string contains the numbers listed, return false and if it does not, return true.
            if ("1234567890".ToCharArray().Any(c => text.Contains(c)))
            {
                return false;

            }
            else
                return true;

        }
    }
}
