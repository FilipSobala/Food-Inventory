using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    // Adres Twojego działającego API
    private static readonly string baseUrl = "http://localhost:5140/api/products"; 

    static async Task Main(string[] args)
    {
        using var client = new HttpClient();
        
        // Nagłówki autoryzacyjne wymagane przez kryteria oceny
        client.DefaultRequestHeaders.Add("X-Username", "admin");
        client.DefaultRequestHeaders.Add("X-Api-Token", "ADMIN-SUPER-SECRET-TOKEN-123");

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================================");
            Console.WriteLine("    PANEL KLIENTA REST API - ZARZĄDZANIE LODÓWKĄ       ");
            Console.WriteLine("========================================================");
            Console.ResetColor();
            Console.WriteLine("1. [GET]  Wyświetl produkty w lodówce (Sformatowane)");
            Console.WriteLine("2. [POST] Dodaj nowy produkt przez API");
            Console.WriteLine("3. Wyjście z programu");
            Console.WriteLine("--------------------------------------------------------");
            Console.Write("Wybierz opcję (1-3): ");

            var menuWybor = Console.ReadLine();

            if (menuWybor == "1")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Pobieranie danych z REST API (http://localhost:5140/api/products)...");
                Console.ResetColor();

                try
                {
                    var response = await client.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var rawJson = await response.Content.ReadAsStringAsync();
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[POBRANO POMYŚLNIE] Zawartość lodówki:");
                        Console.ResetColor();
                        
                        // Zamiast brzydkiego JSONa, drukujemy czytelną dla człowieka tabelę:
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine(string.Format("| {0,-5} | {1,-20} | {2,-10} | {3,-10} | {4,-15} |", "ID API", "Nazwa produktu", "Ilość", "Jednostka", "Data ważności"));
                        Console.WriteLine("----------------------------------------------------------------------------------");

                        // Prymitywne, ale niezawodne parsowanie prostego JSONa bez dodawania zewnętrznych bibliotek
                        var items = rawJson.Trim('[', ']').Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        if (rawJson.Trim() == "[]" || items.Length == 0)
                        {
                            Console.WriteLine("|                      Lodówka jest pusta na tym koncie...                       |");
                        }
                        else
                        {
                            foreach (var item in items)
                            {
                                string cleanItem = item.Replace("{", "").Replace("}", "").Replace("\"", "");
                                var fields = cleanItem.Split(',');

                                string id = fields.FirstOrDefault(f => f.StartsWith("id:"))?.Split(':')[1] ?? "-";
                                string name = fields.FirstOrDefault(f => f.StartsWith("productName:"))?.Split(':')[1] ?? "-";
                                string qty = fields.FirstOrDefault(f => f.StartsWith("quantity:"))?.Split(':')[1] ?? "-";
                                string unit = fields.FirstOrDefault(f => f.StartsWith("unit:"))?.Split(':')[1] ?? "-";
                                string expiry = fields.FirstOrDefault(f => f.StartsWith("expiryDate:"))?.Split(new char[] { ':' }, 2)[1] ?? "-";
                                if(expiry.Length > 10) expiry = expiry.Substring(0, 10); // tylko data, bez czasu

                                Console.WriteLine(string.Format("| {0,-6} | {1,-20} | {2,-10} | {3,-10} | {4,-15} |", id, name, qty, unit, expiry));
                            }
                        }
                        Console.WriteLine("----------------------------------------------------------------------------------");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Błąd API: {response.StatusCode}");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd połączenia: {ex.Message}. Upewnij się, że strona www działa!");
                }
                
                Console.WriteLine("\nWciśnij dowolny klawisz, aby wrócić do menu...");
                Console.ReadKey();
            }
            else if (menuWybor == "2")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("DODAWANIE PRODUKTU PRZEZ REST API (POST)");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("Podaj ID produktu ze słownika (np. 1 dla Jajek, 4 dla Mleka): ");
                string prodId = Console.ReadLine();

                Console.Write("Podaj ilość (np. 4,5): ");
                string ilosc = Console.ReadLine().Replace(",", ".");

                Console.Write("Podaj jednostkę (np. szt, l, kg): ");
                string jedn = Console.ReadLine();

                // Tworzymy surowy obiekt JSON w tle
                string jsonPayload = $"{{\"productId\":{prodId},\"quantity\":{ilosc},\"unit\":\"{jedn}\",\"expiryDate\":\"2026-06-15T00:00:00\",\"notes\":\"Dodano przez Konsolę REST\"}}";

                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nWysyłanie żądania POST do API...");
                Console.ResetColor();

                var response = await client.PostAsync(baseUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[SUKCES] Serwer zaakceptował dane! Produkt trafił do bazy.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[BŁĄD] Serwer odrzucił żądanie: {response.StatusCode}");
                    Console.ResetColor();
                }

                Console.WriteLine("\nWciśnij dowolny klawisz, aby wrócić do menu...");
                Console.ReadKey();
            }
            else if (menuWybor == "3")
            {
                break;
            }
        }
    }
}