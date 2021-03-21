using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PIV_2
{
    static class CRUD
    {
        enum SearchBy
        {
            Name,
            ID,
            unknown
        }
        public static void Read(string connectionString)
        {
            var commandText = "SELECT * FROM dbo.Pracownicy";

            var employees = DapperHelper.ExecuteQuery(connectionString, commandText); //yield zwraca po kazdym wierszu tabeli
            employees.ToList().ForEach(x => Console.WriteLine(x.ToString())); //wyswietlanie zawartosci listy pracownikow
        }
        public static void Create(string connectionString)
        {
            var maxIDemp = DapperHelper.ExecuteQuery(connectionString,
                "SELECT * FROM dbo.Pracownicy"
                ).ToList().Select(x => x.IDpracownika).Max(); //najwyzsze ID
            maxIDemp++;

            #region inicjalizacja parametrow

            DapperHelper.AddParameter("@IDPracownika", maxIDemp.ToString());
            Console.WriteLine("Podaj imię nowego pracownika: ");
            var name = Console.ReadLine();
            DapperHelper.AddParameter("Imię", name);
            Console.WriteLine("Podaj nazwisko: ");
            var lname = Console.ReadLine();
            DapperHelper.AddParameter("Nazwisko", lname);
            Console.WriteLine("Podaj stanowisko: ");
            var position = Console.ReadLine();
            DapperHelper.AddParameter("Stanowisko", position);
            #endregion
            var commandText = "INSERT INTO dbo.Pracownicy (IDpracownika, Nazwisko, Imię, Stanowisko) VALUES (@IDpracownika, @Nazwisko, @Imię, @Stanowisko)";
            DapperHelper.ExecuteNonQuery(connectionString, commandText);
        }
        public static void Update(string connectionString)
        {
            Console.WriteLine("Edytowanie pracownika");
            var searchBy = Search(connectionString);
            Console.WriteLine("Podaj nowe stanowisko pracy");
            var position = Console.ReadLine();
            DapperHelper.AddParameter("Stanowisko", position);
            if (searchBy == SearchBy.Name)
            {
                var commandText = "UPDATE dbo.Pracownicy SET Stanowisko = @stanowisko WHERE Nazwisko = @nazwisko";

                DapperHelper.ExecuteNonQuery(connectionString, commandText);
            }
            else if (searchBy == SearchBy.ID)
            {
                var commandText = "UPDATE dbo.Pracownicy SET Stanowisko = @stanowisko WHERE IDpracownika = @id";

                DapperHelper.ExecuteNonQuery(connectionString, commandText);
            }
        }
        public static void Delete(string connectionString)
        {
            Console.WriteLine("Usuwanie pracownika");
            var searchBy = Search(connectionString);
            if (searchBy == SearchBy.Name)
            {
                var commandText = "DELETE FROM dbo.Pracownicy WHERE Nazwisko = @nazwisko";

                DapperHelper.ExecuteNonQuery(connectionString, commandText);
            }
            else if (searchBy == SearchBy.ID)
            {
                var commandText = "DELETE FROM dbo.Pracownicy WHERE IDpracownika = @id";

                DapperHelper.ExecuteNonQuery(connectionString, commandText);
            }
        }
        private static SearchBy Search(string connectionString)
        {
            var finded = false;
            while (!finded)
            {
                Console.WriteLine("Podaj nazwisko pracownika");
                var name = Console.ReadLine();
                DapperHelper.AddParameter("nazwisko", name);
                var commandText = "SELECT * FROM dbo.Pracownicy WHERE Nazwisko = @nazwisko";

                var findedEmp = DapperHelper.ExecuteQuery(connectionString, commandText).ToList();
                if (findedEmp.Count == 1)
                {
                    return SearchBy.Name;
                }
                else if (findedEmp.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono pracownika, sprobuj ponownie");
                }
                else if (findedEmp.Count > 1)
                {
                    Console.WriteLine($"Znaleziono {findedEmp.Count} pracownikow o nazwisku {name}, wybierz ID poszukiwanego");
                    findedEmp.ForEach(x => Console.WriteLine(x.ToString()));

                    var empWasFouned = false;
                    var success = false;
                    int correctID = 0;

                    while (!empWasFouned)
                    {
                        var empID = Console.ReadLine();
                        success = Int32.TryParse(empID, out correctID);
                        if (!success)
                        {
                            Console.WriteLine("Nieprawidlowa wartosc ID, sprobuj ponownie:");
                        }
                        else
                        {
                            for (int i = 0; i < findedEmp.Count; i++)
                            {
                                if (findedEmp[i].IDpracownika == correctID)
                                {
                                    empWasFouned = true;
                                }
                            }
                            if (!empWasFouned)
                            {
                                Console.WriteLine("Nie znaleziono pracownika o podanym ID, sprobuj ponownie");
                            }
                        }
                    }
                    DapperHelper.AddParameter("id", correctID.ToString());
                    return SearchBy.ID;
                }
            }
            return SearchBy.unknown;
        }

    }
}
