using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.Common;
using System.Configuration;

namespace DataBaseConverter
{
    class Program
    {
       public enum EnumnomeTabela {
            tb_aplicacaoauditada,
            tb_errologauditoria,
            tb_logauditoria
        }
        static string connectionString = ConfigurationManager.AppSettings["ConnectionStringSQL"];
        static string mongoConnectionString = ConfigurationManager.AppSettings["ConnectionStringMongo"];
        static string MongoDataBaseName = ConfigurationManager.AppSettings["MongoDataBaseName"];
        static string MongoCollectionName = ConfigurationManager.AppSettings["MongoCollectionName"];


        static void Main(string[] args)
        {
           
            OpenConnectionAndRun<LogAuditoria>(EnumnomeTabela.tb_logauditoria,1);
            Console.ReadKey();
                
            
        
        }
        public static void OpenConnectionAndRun<T>(EnumnomeTabela nomeTabela,int id_aplicacaoauditada)where T:IDisposable,IPropriedadesSql,new() {
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Abrindo conexão com o banco de dados...");
            try
            {
                connection.Open();
                Task.Run(() => GetData<T>(connection, nomeTabela, id_aplicacaoauditada));
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro ao conectar com o banco de dados,\ncertifique-se do acesso e da permissão da maquina atual com o banco");
            }
            catch (ArgumentNullException ex) {
                Console.WriteLine("Erro de argumento nulo:"+ex.Message);
            }
        }
        public static string CreateDataDirectory(EnumnomeTabela nome) {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string path = Path.Combine(systemPath, "DataBaseConverter");
            Directory.CreateDirectory(path);
            string s = path + "/" + nome + ".txt";
            if (!File.Exists(s))
            using (var stream =File.Create(s)) {
                    using (StreamWriter st = new StreamWriter(stream)) {
                        st.WriteLine("0");
                    }                      
                s = stream.Name;
            }
            return s;
        }
        public static int GetID(string path) {
            string s = File.ReadAllText(path);
           return Int32.Parse(s);
            
        }
        public static async Task GetData<T>(SqlConnection connection,EnumnomeTabela nomeTabela,int id_aplicacaoauditada) where T: IDisposable,IPropriedadesSql,new() {
            DateTime START = DateTime.Now;
            MongoClient mongoClient = new MongoClient(mongoConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(MongoDataBaseName);
            IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(MongoCollectionName);
            using (var command = new SqlCommand(@"Select * FROM [LogAuditoria].[dbo].[" + nomeTabela + "] where id_aplicacaoauditada="+id_aplicacaoauditada, connection))
            {
             
                using (var c = await command.ExecuteReaderAsync())
                {
                    c.AsParallel();
                    int count = c.FieldCount;
                    int countTimes = 0;                    
                    try
                    {
                        ParallelDownload<T>(mongoCollection,count,countTimes,c);
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Erro ao baixar de forma paralela,\nconsidere aumentar o valor CommandTimeout no banco de dados\nou espere as conexões diminuirem");
                    }
                    Console.WriteLine("Importação da tabela:"+nomeTabela+"concluida em:"+ (DateTime.Now - START));
                    connection.CloseAsync();
                }
            }
        }
        public static void ParallelDownload<T>(IMongoCollection<BsonDocument> mongoCollection,int count,int countTimes,SqlDataReader c) where T : IDisposable, IPropriedadesSql, new()
        {
            List<T> Lista = new List<T>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.ForEach(c.Cast<DbDataRecord>(), options, async (record) =>
            {
                List<string> propriedades = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    propriedades.Add(record.GetValue(i).ToString());

                }
                countTimes++;
                using (T instance = new T())
                {
                    instance.PopulatePropriedades(propriedades);
                     Lista.Add(instance);
                    Console.WriteLine("Arquivo encontrado e baixado: "+countTimes);
                }

                //   }
            });
            Lista =Lista.OrderBy(x =>x.id()).ToList();
            List<BsonDocument> b = Lista.ConvertAll(x => x.ToBsonDocument());
            mongoCollection.InsertMany(b);
        }
    }
}
