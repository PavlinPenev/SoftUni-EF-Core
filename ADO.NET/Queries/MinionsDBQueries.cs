namespace ADO.NET.Queries
{
    public static class MinionsDBQueries
    {
        public static class Task01Queries
        {
            public const string CREATE_TABLES_MINIONS_DB_QUERY =
            @"CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))

            CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))

            CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))

            CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))

            CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))

            CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";


            public const string INSERT_INTO_TABLES_IN_MINIONS_DB =
                @"INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')

            INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)

            INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)

            INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')

            INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)

            INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";
        }

        public static class Task02Queries
        {
            public const string SELECT_VILLAIN_AND_THEIR_MINIONS_COUNT_QUERY =
            @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                FROM Villains AS v 
                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
            GROUP BY v.Id, v.Name 
              HAVING COUNT(mv.VillainId) > 3 
            ORDER BY COUNT(mv.VillainId)";
        }

        public static class Task03Queries
        {
            public const string SELECT_VILLAIN_QUERY =
            @"SELECT Name FROM Villains WHERE Id = @Id";

            public const string SELECT_VILLAIN_MINIONS_QUERY =
                @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                     m.Name, 
                                                     m.Age
                                                FROM MinionsVillains AS mv
                                                JOIN Minions As m ON mv.MinionId = m.Id
                                               WHERE mv.VillainId = @Id
                                            ORDER BY m.Name";
        }
        
        public static class Task04Queries
        {
            public const string SELECT_VILLAIN_BY_NAME_QUERY =
            @"SELECT Id FROM Villains WHERE Name = @Name";

            public const string SELECT_MINIONS_ID_BY_NAME_QUERY =
                @"SELECT Id FROM Minions WHERE Name = @Name";

            public const string SELECT_TOWN_BY_NAME_QUERY =
                @"SELECT Id FROM Towns WHERE Name = @townName";

            public const string INSERT_TOWN_QUERY =
                @"INSERT INTO Towns (Name) VALUES (@townName)";

            public const string INSERT_VILLAIN_QUERRY =
                @"INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, 4)";

            public const string INSERT_MINION_QUERY =
                @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            public const string ASSIGN_MINION_TO_VILLAIN_QUERY =
                @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
        }  

        public static class Task05Queries
        {
            public const string UPDATE_TOWNS_QUERY =
                @"UPDATE Towns
                   SET Name = UPPER(Name)
                 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            public const string SELECT_TOWNS_BY_GIVEN_COUNTRY_QUERY =
                @"SELECT t.Name 
                FROM Towns as t
                JOIN Countries AS c ON c.Id = t.CountryCode
                WHERE c.Name = @countryName";
        }

        public static class Task06Queries
        {
            public const string SELECT_VILLAINS_NAME_QUERY =
                @"SELECT Name FROM Villains WHERE Id = @villainId";

            public const string DELETE_VILLAIN_QUERY =
                @"DELETE FROM MinionsVillains 
                        WHERE VillainId = @villainId

                DELETE FROM Villains
                        WHERE Id = @villainId";

            public const string GET_COUNT_OF_VILLAINS_MINIONS_QUERY =
                @"SELECT COUNT(*)
                    FROM MinionsVillains AS mv
                    JOIN Minions As m ON mv.MinionId = m.Id
                    WHERE mv.VillainId = @villainId";
        }

        public static class Task07Queries
        {
            public const string SELECT_ALL_MINION_NAMES_QUERY =
                @"SELECT Name FROM Minions";
        }

        public static class Task08Queries
        {
            public const string SELECT_MINION_NAMES_AND_AGE_QUERY =
                @"SELECT Name, Age FROM Minions";

            public const string UPDATE_MINIONS_AGE_QUERY =
                @"UPDATE Minions
                   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                 WHERE Id = @Id";
        }

        public static class Task09Queries
        {
            public const string CREATE_PROCEDURE_GET_OLDER_QUERY =
                @"CREATE OR ALTER PROC usp_GetOlder @id INT
                AS
                UPDATE Minions
                   SET Age += 1
                 WHERE Id = @Id";

            public const string EXEC_PROCEDURE_GET_OLDER_QUERY =
                @"EXEC dbo.usp_GetOlder @Id";

            public const string SELECT_MINION_NAMES_AND_AGE_QUERY =
                @"SELECT Name, Age FROM Minions WHERE Id = @Id";
        }
    }
}
