using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.IO;

namespace classTestailua
{
    class Program
    {
        static void Main(string[] args)
        {
        //Start of program
        programStart:
            //CreateNewLine or clear based on if an empty line exists or not. This is to help with id creation of person.
            ClearAfterInput("Hei! Aloita valitsemalla haluatko lisätä uuden henkilön vai tarkastella olemassa olevia:");

            Console.WriteLine("Uusi henkilö: paina 1 ja Enter");
            Console.WriteLine("Olemassa olevat henkilöt: paina 2 ja Enter");
            
            int startSelectionFaiLimit = 0;
        userInputStart:
            //Input which is tryparsed, if it fails come back to input and otherwise continue the program
            string userInput = Console.ReadLine();
            int InputNumber;
            if (!int.TryParse(userInput, out InputNumber))
            {
                Console.WriteLine("Syötä vain numeroita:");
                startSelectionFaiLimit++;
                if(CheckFail(startSelectionFaiLimit) == true)
                {
                    goto programStart;

                }
                goto userInputStart;
            }
            else

                switch (userInput)
                {
                    case "1":
                    //Start of giving name
                    GiveName:
                        Human newHuman = new Human();
                        Console.Clear();
                        Console.WriteLine("Mikäs mahtaa olla kyseisen henkilön nimi?");
                        
                        int nameFailLimit = 0;
                    GiveNameOnFail:
                        newHuman.Name = Console.ReadLine();
                        if (Human.CheckIfDoesNotContainNumbers(newHuman.Name) != true)
                        {
                            Console.WriteLine("Antamassasi nimessä on numeroita. Syötä vain kirjaimia:");
                            nameFailLimit++;
                            if(CheckFail(nameFailLimit) == true)
                            {
                                goto GiveName;
                            }
                            goto GiveNameOnFail;
                        }
                        if(newHuman.Name == "")
                        {
                            Console.WriteLine("Nimi ei voi olla tyhjä, yritä uudelleen:");
                            nameFailLimit++;
                            if (CheckFail(nameFailLimit) == true)
                            {
                                goto GiveName;
                            }
                            goto GiveNameOnFail;
                        }
                    GiveAge:
                        ClearAfterInput($"Kuinka vanha on {newHuman.Name}?");
                        
                        int ageFailLimit = 0;
                    //Start of giving age, if age contains letters or it is over 125, start over
                    GiveAgeOnFail:
                        string age = Console.ReadLine();
                        int ageNum;
                        if (!int.TryParse(age, out ageNum))
                        {
                            Console.WriteLine("Antamassassi iässä on kirjaimia. Syötä vain numeroita:");
                            ageFailLimit++;
                            if (CheckFail(ageFailLimit) == true)
                            {
                                goto GiveAge;
                            }
                            goto GiveAgeOnFail;
                        }

                        if (ageNum > 125)
                        {
                            Console.WriteLine($"{age}? Tuota en kyllä usko. Anna hänen oikea ikä.");
                            ageFailLimit++;
                            if (CheckFail(ageFailLimit) == true)
                            {
                                goto GiveAge;
                            }
                            goto GiveAgeOnFail;
                        }
                        newHuman.Age = ageNum;

                    GiveGender:
                        ClearAfterInput("Mikä on hänen sukupuolensa? Valitse mies painamalla 1 tai nainen painamalla 2:");
                        
                        int genderFailLimit = 0;
                    //Start of giving gender
                    GiveGenderOnFail:
                        string genderInput = Console.ReadLine();
                        int GenderNum;
                        if (!int.TryParse(genderInput, out GenderNum))
                        {
                            Console.WriteLine("Syötä vain numeroita:");
                            genderFailLimit++;
                            if (CheckFail(genderFailLimit) == true)
                            {
                                goto GiveGender;
                            }
                            goto GiveGenderOnFail;
                        }
                        if (GenderNum == 1)
                        {
                            newHuman.Gender = "mies";
                        }
                        else if (GenderNum == 2)
                        {
                            newHuman.Gender = "nainen";
                        }
                        else if (GenderNum != 1 || GenderNum != 2)
                        {
                            Console.WriteLine("Syötit väärän arvon. Valitse 1 tai 2:");
                            genderFailLimit++;
                            if (CheckFail(genderFailLimit) == true)
                            {
                                goto GiveGender;
                            }
                            goto GiveGenderOnFail;
                        }
                        //Create an ID for the person
                        Human.CreateNewLineAndClear();
                        Human.MakeId(File.ReadAllLines("People.txt").Length, newHuman);
                        //Delimiters for splitting person
                        string[] delimiters = { ": " };
                        
                        string person = newHuman.ToString();
                        string[] personSplit = person.Split(delimiters, StringSplitOptions.None);

                        //Get the ID-number from personSplit
                        //Get all lines from People.txt and split the retrieved item for comparison with personSplit
                        Human.CreateNewLineAndClear();
                        string[] data = File.ReadAllLines("People.txt");
                        int id = data.Length;
                        if (id < data.Length)

                        {
                            id = data.Length;
                        }
                        string item;
                        if (data.Length == 0)
                        {
                            item = data[id++];
                        }
                        else


                        item = data[id - 1];


                        string[] itemSplit = item.Split(delimiters, StringSplitOptions.None);

                        //If statement to check if person and the item already existing in the file have the same ID. If they do, replace person ID with one that has been increased by 1.
                        if (personSplit[0] == itemSplit[0])
                        {
                            string increasedID = personSplit[0].Replace($"ID {id - 1}", $"ID {id}");
                            person = person.Replace(personSplit[0], increasedID);
                        }

                        //After all that, finally write the person into the file and if their id already existed, with the updated one.
                        using (StreamWriter writer = new StreamWriter("People.txt", true))
                        {
                            writer.WriteLine(person);
                        }
                    endOfHuman:
                        ClearAfterInput($"Eli {newHuman.Name} on {newHuman.Age} vuotta vanha {newHuman.Gender}? Kiinnostavaa.");
                        Console.WriteLine();
                        Console.WriteLine("Haluaisitko lisätä uuden henkilön vai palata alkuun? Paina 1 palataksesi tai 2 uuden henkilön lisäykseen.");
                        Console.WriteLine("Voit myös tarkastella tallennttuja henkilöitä painamalla 3.");
                        
                        int selectionFailLimit = 0;
                    endOfHumanIfFail:
                        //End of creating person

                        string exitInput = Console.ReadLine();
                        int exitNum;

                        if (!int.TryParse(exitInput, out exitNum))
                        {
                            Console.WriteLine("Syötä vain numeroita:");
                            selectionFailLimit++;
                            if (CheckFail(selectionFailLimit) == true)
                            {
                                goto endOfHuman;
                            }
                            goto endOfHumanIfFail;
                        }

                        if (exitNum == 1)
                        {
                            goto programStart;
                        }
                        else if (exitNum == 2)
                        {
                            goto GiveName;
                        }
                        else if (exitNum == 3)
                        {
                            goto savedPeople;
                        } else
                        {
                            Console.WriteLine("Syötit väärän arvon. Valitse 1 , 2 tai 3:");
                            selectionFailLimit++;
                            if (CheckFail(selectionFailLimit) == true)
                            {
                                goto endOfHuman;
                            }
                            goto endOfHumanIfFail;
                        }

                    savedPeople:
                        ClearAfterInput("Pystyt muokkaamaan tallennettuja henkilöitä painamalla 1 tai palaamaan takaisin painamalla 2.");
                        Human.CreateNewLineAndClear();
                        Human.GetSavedItems(false, 0);
                        int savedFailLimit = 0;
                    startIfFail:
                        string savedInput = Console.ReadLine();
                        int savedNum;
                        if (!int.TryParse(savedInput, out savedNum))
                        {
                            Console.WriteLine("Syötä vain numeroita:");
                            savedFailLimit++;
                            if(CheckFail(savedFailLimit) == true)
                            {
                                goto savedPeople;
                            }
                            goto startIfFail;
                        }
                        if (savedNum == 1)
                        {
                        editStart:
                            EditSavedItems();
                            goto editStart;
                        }
                        else if (savedNum == 2)
                        {
                            goto programStart;
                        }
                        else
                        {
                            Console.WriteLine("Syötit väärän arvon. Valitse 1 , 2 tai 3:");
                            savedFailLimit++;
                            if(CheckFail(savedFailLimit) == true)
                            {
                                goto savedPeople;
                            }
                            goto startIfFail;
                        }

                    case "2":
                    startOfEditFromStart:
                        ClearAfterInput("Pystyt muokkaamaan tallennettuja henkilöitä painamalla 1 tai palaamaan takaisin painamalla 2.");
                        Human.CreateNewLineAndClear();
                        Human.GetSavedItems(false, 0);
                        int savedStartFailLimit = 0;
                    startOfEditFromStartIfFail:
                        string savedInput2 = Console.ReadLine();
                        int savedNum2;
                        if (!int.TryParse(savedInput2, out savedNum2))
                        {
                            Console.WriteLine("Syötä vain numeroita:");
                            savedStartFailLimit++;
                            if(CheckFail(savedStartFailLimit) == true)
                            {
                                goto startOfEditFromStart;
                            }
                            goto startOfEditFromStartIfFail;
                        }
                        if (savedNum2 == 1)
                        {
                            Human.CreateNewLineAndClear();
                        editStart:
                            EditSavedItems();
                            goto editStart;
                        }
                        else if (savedNum2 == 2)
                        {
                            goto programStart;
                        }
                        else
                        {
                            Console.WriteLine("Syötit väärän arvon. Valitse 1 , 2 tai 3:");
                            savedStartFailLimit++;
                            if (CheckFail(savedStartFailLimit) == true)
                            {
                                goto startOfEditFromStart;
                            }
                            goto startOfEditFromStartIfFail;
                        }

                    default:
                        Console.WriteLine("Syötit väärän arvon. Valitse 1 tai 2:");
                        startSelectionFaiLimit++;
                        if (CheckFail(startSelectionFaiLimit) == true)
                        {
                            goto programStart;
                        }
                        goto userInputStart;
                }

        }

        private static void EditSavedItems()
        {
        SelectItem:
            int id = File.ReadAllLines("People.txt").Length;
            ClearAfterInput("Voit muokata tallennettuja tietoja kirjoittamalla niiden ID-luvun ja painamalla enter.");
            Human.GetSavedItems(false, 0);

            //An int to track number of fails
            int failLimit = 0;

        //Start of selection of item to be edited
        selectItemIfFail:
            string userInput = Console.ReadLine();
            int inputNum;
            if (!int.TryParse(userInput, out inputNum))
            {
                Console.WriteLine("Syötä vain numeroita.");
                failLimit += 1;
                if (failLimit >= 4)
                {
                    goto SelectItem;
                }
                goto selectItemIfFail;
            }
            if (Convert.ToInt32(userInput) >= id || Convert.ToInt32(userInput) < 0)
            {
                Console.WriteLine("Syötettyä arvoa ei ole olemassa.");
                failLimit += 1;
                if (failLimit >= 4)
                {
                    goto SelectItem;
                }
                goto selectItemIfFail;
            }
            int selectedItem = Convert.ToInt32(userInput);

        editStart:
            ClearAfterInput("Muokkaat: ");
            Human.GetSavedItems(true, selectedItem);
            //Start of actual editing
            Console.WriteLine("Voit muokata nimeä painamalla 1, ikää painamalla 2 tai sukupuolta painamalla 3. Poista painamalla 0. Voit myös palata takaisin painamalla 4.");
        editStartIfFail:
            userInput = Console.ReadLine();
            if (!int.TryParse(userInput, out inputNum))
            {
                Console.WriteLine("Syötä vain numeroita.");
                failLimit += 1;
                if (failLimit >= 4)
                {
                    goto editStart;
                }
                goto editStartIfFail;
            };

            if (inputNum == 1)
            {
                Human.editName("People.txt", selectedItem, new[] { ": ", ", " });
                goto editStart;
            }
            else if (inputNum == 2)
            {
                Human.editAge("People.txt", selectedItem, new[] { ": ", ", " });
                goto editStart;
            }
            else if (inputNum == 3)
            {
                Human.editGender("People.txt", selectedItem, new[] { ": ", ", " });
                goto editStart;
            }
            else if (inputNum == 4)
            {

            }
            else if (inputNum == 0)
            {
            select:
                ClearAfterInput($"Oletko varma että haluat poistaa:");
                Human.GetSavedItems(true, selectedItem);
                Console.WriteLine("1: KYLLÄ");
                Console.WriteLine("2: EI");
                int deleteFailLimit = 0;
            selectIfFail:
                string selectDel = Console.ReadLine();
                int num;
                if (!int.TryParse(selectDel, out num))
                {
                    Console.WriteLine("Syötä vain numeroita.");
                    deleteFailLimit += 1;
                    if (deleteFailLimit >= 4)
                    {
                        goto select;
                    }
                    goto selectIfFail;
                }
                if (num == 1)
                {
                    Human.Delete("People.txt", selectedItem);
                } else if (num == 2)
                {
                    goto editStart;
                } else
                {
                    Console.WriteLine("Syötit väärän arvon.Valitse 1 tai 2:");
                    failLimit += 1;
                    if (failLimit >= 4)
                    {
                        goto select;
                    }
                    goto selectIfFail;
                }
            }
        }

        public static void ClearAfterInput(string afterClearText)
        {
            //Clear the line and write the designated string
            Console.Clear();
            Console.WriteLine(afterClearText);

        }

        public static bool CheckFail(int fails)
        {
            //Function to handle user input being wrong
            if (fails == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
