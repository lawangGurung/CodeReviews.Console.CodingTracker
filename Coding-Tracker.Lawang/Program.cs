using System.Configuration;
using Lawang.Coding_Tracker;


string cs = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
var dbManager = new DatabaseManager(cs);

dbManager.CreateTable();

// Data is seeded into the table only when table is empty.
dbManager.SeedDataIntoTable();

var userInput = new UserInput();

userInput.MainMenu();


