using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;

namespace Gear_Score_New_name
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string ClientSecret = "client_secret_Failures_Besti.json";
        private static readonly string[] ScopesSheets = { SheetsService.Scope.Spreadsheets };
        private static readonly string AppName = "Failures_Besti";
        private static readonly string SpreadsheetId = "1WA-TtO2Do4Nu2S-nDjWAgxDRMhLe-evV6kncARZyJOU";
        private const string Range = "!A1";
        private const string Range2 = "!A2";
        private const string Range3 = "!A3";
        private const string Range4 = "!A4";
        private const string Range5 = "!A5";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Butt_Besty_Click(object sender, RoutedEventArgs e)
        {
            butt_Besty.IsEnabled = false;
            if (butt_Besty.IsEnabled == false)
            {
                butt_Armed.IsEnabled  = true;
                butt_Pro100.IsEnabled = true;
                butt_Setos.IsEnabled  = true;
                butt_Gyydie.IsEnabled = true;
            }

            var credential = GetSheetCredentials();

            var service = GetService(credential);

            string result = GetFirestCell(service, Range, SpreadsheetId);  //получение данных 1q строки 1стобца из таблицы 

            faulesText.Text = Convert.ToString(result);
        }

        private void Butt_Armed_Click(object sender, RoutedEventArgs e)
        {
            butt_Armed.IsEnabled      = false;
            if (butt_Armed.IsEnabled == false)
            {
                butt_Besty.IsEnabled  = true;
                butt_Pro100.IsEnabled = true;
                butt_Setos.IsEnabled  = true;
                butt_Gyydie.IsEnabled = true;
            }


            var credential  = GetSheetCredentials();

            var service     = GetService(credential);

            string result   = GetFirestCell(service, Range, SpreadsheetId);//получение данных 2й строки 1 столбца из таблицы

            faulesText.Text = Convert.ToString(result);
        }

        private void Butt_Pro100_Click(object sender, RoutedEventArgs e)
        {
            butt_Pro100.IsEnabled     = false;
            if (butt_Armed.IsEnabled == false)
            {
                butt_Besty.IsEnabled  = true;
                butt_Armed.IsEnabled  = true;
                butt_Setos.IsEnabled  = true;
                butt_Gyydie.IsEnabled = true;
            }

            var credential  = GetSheetCredentials();

            var service     = GetService(credential);

            string result   = GetFirestCell(service, Range3, SpreadsheetId);//получение данных 3й строки 1 столбца из таблицы

            faulesText.Text = Convert.ToString(result);
        }

        private void Butt_Setos_Click(object sender, RoutedEventArgs e)
        {
            butt_Setos.IsEnabled      = false;
            if (butt_Armed.IsEnabled == false)
            {
                butt_Besty.IsEnabled  = true;
                butt_Pro100.IsEnabled = true;
                butt_Armed.IsEnabled  = true;
                butt_Gyydie.IsEnabled = true;
            }

            var credential  = GetSheetCredentials();

            var service     = GetService(credential);

            string result   = GetFirestCell(service, Range4, SpreadsheetId);  //получение данных 4й строки 1стобца из таблицы 

            faulesText.Text = Convert.ToString(result);
        }

        private void Butt_Gyydie_Click(object sender, RoutedEventArgs e)
        {
            butt_Gyydie.IsEnabled     = false;
            if (butt_Armed.IsEnabled == false)
            {
                butt_Besty.IsEnabled  = true;
                butt_Pro100.IsEnabled = true;
                butt_Setos.IsEnabled  = true;
                butt_Armed.IsEnabled  = true;
            }

            var credential  = GetSheetCredentials();

            var service     = GetService(credential);

            string result   = GetFirestCell(service, Range5, SpreadsheetId);  //получение данных 5й строки 1стобца из таблицы 

            faulesText.Text = Convert.ToString(result);
        }


        private static UserCredential GetSheetCredentials()// метод чтения файло json
        {
            using (var stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                var credPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "sheetsCreds.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, ScopesSheets, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;

            }
        }

        private static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }

        private static void FillSpreadsheet(SheetsService service, string spreadsheetId, string data)
        {
            List<Request> requests = new List<Request>();

            List<CellData> values = new List<CellData>();

            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = data
                }
            });

            requests.Add(
                new Request
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Start = new GridCoordinate
                        {
                            SheetId = 0,
                            RowIndex = 0,
                            ColumnIndex = 0
                        },
                        Rows = new List<RowData> { new RowData { Values = values } },
                        Fields = "userEnteredValue"
                    }
                }

            );


            BatchUpdateSpreadsheetRequest bust = new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            };


            service.Spreadsheets.BatchUpdate(bust, spreadsheetId).Execute();
        }// запись данных в 1ю строку

        public static string GetFirestCell(SheetsService service, string range, string spreadsheetId)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();

            string result = null;

            foreach (var value in response.Values)
            {
                result += " " + value[0];
            }

            return result;
        }


    }
}
