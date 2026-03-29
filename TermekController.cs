using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;

namespace WebshopAPI.Controllers
{
    [ApiController]
    [Route("api/termekek")]
    public class TermekController : ControllerBase
    {

        string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                                  ?? "server=127.0.0.1;user=root;password=mysql;database=webaruhaz";

        [HttpGet]
        public List<object> GetTermekek()
        {

            var lista = new List<object>();

            using var kapcsolat = new MySqlConnection(connectionString);
            kapcsolat.Open();

            var cmd = new MySqlCommand("SELECT * FROM adatok", kapcsolat);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new
                {
                    id = reader.GetInt32("id"),
                    tipus = reader.GetString("tipus"),
                    kep = reader.GetString("kep"),
                    gyarto = reader.GetString("gyarto"),
                    model = reader.GetString("termek_model"),
                    ar = reader.GetInt32("ar"),
                    leiras = reader.GetString("leiras")
                });
            }

            return lista;
        }

        [HttpDelete("{id}")]
        public string Torles(int id)
        {
            using var kapcsolat = new MySqlConnection(connectionString);
            kapcsolat.Open();

            var cmd = new MySqlCommand("DELETE FROM adatok WHERE id=@id", kapcsolat);
            cmd.Parameters.AddWithValue("@id", id);

            int sor = cmd.ExecuteNonQuery();

            if (sor > 0)
                return "Termék törölve";
            else
                return "Nincs ilyen termék";
        }

        [HttpGet("{id}")]
        public object GetTermek(int id)
        {
            using var kapcsolat = new MySqlConnection(connectionString);
            kapcsolat.Open();

            var cmd = new MySqlCommand("SELECT * FROM adatok WHERE id=@id", kapcsolat);
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new
                {
                    id = reader.GetInt32("id"),
                    tipus = reader.GetString("tipus"),
                    kep = reader.GetString("kep"),
                    gyarto = reader.GetString("gyarto"),
                    model = reader.GetString("termek_model"),
                    ar = reader.GetInt32("ar"),
                    leiras = reader.GetString("leiras"),
                    tech = reader.GetString("technikai_ertek")
                };
            }

            return null;
        }
    }


}