using System;

namespace MP3ManagerApplication.Pages.UI
{
    class OrganizeUI : IUserInterface
    {
        private int choice;
        private SingleFileUI singleFileUI;
        private MultipleFilesUI multipleFilesUI;

        private static OrganizeUI organizeUI;


        private OrganizeUI()
        {

        }

        public static OrganizeUI start()
        {
            if (organizeUI == null)
            {
                organizeUI = new OrganizeUI();
            }

            return organizeUI;
        }

        public void Run(ref MP3Engine mp3Engine)
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Would you like to organize:" +
                    "\n1- a single file" +
                    "\n2- a multiple files" +
                    "\n3- return to The main menu");

                choice = Prog.input();


                if (choice == 1)
                {
                    singleFileUI = SingleFileUI.start();
                    singleFileUI.Run(ref mp3Engine);

                    if (singleFileUI.isOrganized == true)
                    {
                        singleFileUI.Dispose();
                        break;
                    }

                    singleFileUI.Dispose();
                }
                else if (choice == 2)
                {
                    multipleFilesUI = MultipleFilesUI.start();
                    multipleFilesUI.Run(ref mp3Engine);

                    if (multipleFilesUI.isOrganized == true)
                    {
                        multipleFilesUI.Dispose();
                        break;
                    }

                    multipleFilesUI.Dispose();
                }
                else if (choice == 3)
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Prog.setWarningMessage("Please select one of the options to Continue.\n");
                }
            }

            Console.Clear();
        }

        public void Dispose()
        {
            organizeUI = null;
        }

    }
}
