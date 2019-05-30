using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SupremeCourtDocketApp.Models.CaptureDocketsFromDb
{
    public class SeedDb
    {
        private readonly SupremeCourtDocketApp.Models.SupremeCourtDocketSimpleAppContext _context_simple;
        private readonly SupremeCourtDocketApp.Models.SupremeCourtDocketAppContext _context;
        //public DocketsSimpleModel(SupremeCourtDocketApp.Models.SupremeCourtDocketSimpleAppContext context)
        //{
        //    _context = context;
        //}

        public IList<SupremeCourtDocketSimple> SupremeCourtDocketSimple { get; set; }
        public IList<SupremeCourtDocket> SupremeCourtDocket { get; set; }

        //public async Task OnGetAsync()
        //{
        //    SupremeCourtDocketSimple = await _context.SupremeCourtDocketSimple.ToListAsync();
        //}

        //internal static void TransferData()
        //{
        //    SupremeCourtDocketSimple = _context.SupremeCourtDocketSimple.ToList();






        //}


        internal static void Initialize()
        {
            using (var cxn = new SqlConnection())
            {
                cxn.ConnectionString =
                    "Data Source=CLB15020\\SQLEXPRESS;Initial Catalog=ScotusDocketsCopy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                //Data Source=CLB15020\SQLEXPRESS;Initial Catalog=ScotusDocketsCopy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False



                cxn.Open();
            }




            var options = new DbContextOptions<SupremeCourtDocketAppContext>();
            using (var context = new SupremeCourtDocketAppContext(options))
            {
                using (var cxn = new SqlConnection())
                {
                    cxn.ConnectionString =
                        "Data Source=CLB15020\\SQLEXPRESS;Initial Catalog=ScotusDocketsCopy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; ;

                    //Data Source=CLB15020\SQLEXPRESS;Initial Catalog=ScotusDocketsCopy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False



                    cxn.Open();

                    // create a SqlCommand object for this connection
                    SqlCommand command = cxn.CreateCommand();
                    command.CommandText = "Select "
                        + "DocketNumber,WebAddress,"
                        + "WebPage,DateRetrieved "
                        + "from SupremeCourtDocket";


                    int i = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var scd = new SupremeCourtDocket()
                                {
                                    //ID = last_id,
                                    DocketNumber = reader[0].ToString(),
                                    WebAddress = reader[1].ToString(),
                                    WebPage = reader[2].ToString(),
                                    DateRetrieved = DateTime.Today
                                };
                                scd.SetExtendedProperties();
                                context.Add(scd);
                                i++;
                                if (i >= 100) break;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");
                        }
                        reader.Close();
                    }
                }
                context.SaveChanges();
            }
        }

        internal static async void TransferData(IServiceProvider services)
        {
            IList<SupremeCourtDocketSimple> supremeCourtDocketSimple;
            IList<SupremeCourtDocket> supremeCourtDocket;
            var i = 0;
            using (var context_simple = services.GetRequiredService<Models.SupremeCourtDocketSimpleAppContext>())
            using (var context = services.GetRequiredService<Models.SupremeCourtDocketAppContext>())
            {
                supremeCourtDocketSimple = await context_simple.SupremeCourtDocketSimple.ToListAsync();
                supremeCourtDocket = await context.SupremeCourtDocket.ToListAsync();
                foreach (var d in supremeCourtDocketSimple)
                {
                    if (!supremeCourtDocket.Any(x => x.DocketNumber.Equals(d.DocketNumber)))
                    {

                        var scd = new SupremeCourtDocket()
                        {
                            //ID = last_id,
                            DocketNumber = d.DocketNumber,
                            WebAddress = d.WebAddress,
                            WebPage = d.WebPage,
                            DateRetrieved = d.DateRetrieved
                        };
                        scd.SetExtendedProperties();
                        i++;
                        if (i >= 100) break;
                        context.Add(scd);

                    }
                }

                context.SaveChanges();
            }
            

        }

        internal static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Models.SupremeCourtDocketAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Models.SupremeCourtDocketAppContext>>()))
            {
                using (var cxn = new SqlConnection())
                {
                    cxn.ConnectionString =
                        "Data Source=CLB15020\\SQLEXPRESS;" +
                        "Initial Catalog=ScotusDocketsCopy;" +
                        "Integrated Security=True;" +
                        "Connect Timeout=30;" +
                        "Encrypt=False;" +
                        "TrustServerCertificate=False;" +
                        "ApplicationIntent=ReadWrite;" +
                        "MultiSubnetFailover=False";

                    //Data Source=CLB15020\SQLEXPRESS;Initial Catalog=ScotusDocketsCopy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False



                    cxn.Open();

                    // create a SqlCommand object for this connection
                    SqlCommand command = cxn.CreateCommand();
                    command.CommandText = "Select "
                        + "DocketNumber,WebAddress,"
                        + "WebPage,DateRetrieved "
                        + "from SupremeCourtDocket";


                    int i = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var scd = new SupremeCourtDocket()
                                {
                                    //ID = last_id,
                                    DocketNumber = reader[0].ToString(),
                                    WebAddress = reader[1].ToString(),
                                    WebPage = reader[2].ToString(),
                                    DateRetrieved = DateTime.Today
                                };
                                scd.SetExtendedProperties();
                                context.Add(scd);
                                i++;
                                if (i >= 100) break;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");
                        }
                        reader.Close();
                    }
                }
                context.SaveChanges();
            }
        }
    }
}