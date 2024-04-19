using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatiaTransfer
{
    public class Axiom
    {
        public static string DefaultConnectionString()
        {

            return "Data Source=.\\MSSQL2012;Initial Catalog=AXIOMDENIZLER2024;Persist Security Info=True;User ID=sa;Password=mssql;MultipleActiveResultSets=True;";

        }
        public static string UpdateDeleteSql(string axiomConnectionString, string sql)
        {
            string sonuc = null;
            try
            {
                string connString = axiomConnectionString != null ? axiomConnectionString : Axiom.DefaultConnectionString();
                string query = sql;

                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.ExecuteNonQuery();
                sonuc = "Başarılı";
            }
            catch (Exception e)
            {
                sonuc = "Hata :" + e.Message;

            }

            return sonuc;

        }
        public static DataTable GetDataTable(string axiomConnectionString, string sql)
        {

            DataTable dataTable = new DataTable();
            string connString = axiomConnectionString != null ? axiomConnectionString : Axiom.DefaultConnectionString();
            string query = sql;

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            return dataTable;
        }
    }
    
    

    public class BOM
    {
        public int id { get; set; }
        public int parentid { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Adi2 { get; set; }
        public string Adi3 { get; set; }
        public int StokTipi { get; set; }
        public int Revizyon { get; set; }
        public int Miktar { get; set; }
        public string BOMBirim { get; set; }
        public int OperasyonSiraNo { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string MakinePersonelGrubuKodu { get; set; }
        public string MakinePersonelGrubuAdi { get; set; }
        public int HazirlikSuresi { get; set; }
        public int OperasyonSuresi { get; set; }
        public int KullanilanSiraNo { get; set; }
        public string KullanilanKodu { get; set; }
        public string KullanilanAdi { get; set; }
        public int KullanilanYedekTip { get; set; }
        public decimal KUllanilanMiktar { get; set; }
        public string KullanilanBirim { get; set; }
        public string BOMAciklama { get; set; }
        public string KullanilanAciklama { get; set; }
    
   
    }

        
    public class Transfer
    {

        
        public static void CreateBom(List<BOM> bom)
        {

            foreach (BOM item in bom)
            {

                CreateItems(item);

                
            }

            string sonuc;
            foreach (BOM item in bom)
            {

                string sorurun = "Select OId from Production_UrunAgaci where StokKodu like'" + item.StokKodu + "' and Parents_Id is null";

                DataTable dt_urun = Axiom.GetDataTable(null, sorurun);

                string parentid = "NULL";
                if (dt_urun.Rows.Count>0)
                {
                    parentid = "'" + dt_urun.Rows[0].ItemArray[0].ToString()+"'";
                }
                else
                {
                    string sql_urun = " INSERT INTO [Production_UrunAgaci] ([OId], [Sira], [StokKodu], [UrunStatus], [Miktar], [IsSonRevizyon], [HazirlikSuresi], [OperasyonSuresi], [Parents_Id], ";
                    sql_urun = sql_urun + "  [Item_Id], [MakinePersonelGrubu_Id], [Sirket_Id]) VALUES (NEWID(), 0, N'" + item.StokKodu + "', 1, " + Convert.ToDecimal(item.Miktar) + ", '1', '1900-01-01 00:00:00.000',   ";
                    sql_urun = sql_urun + " '1900-01-01 00:00:00.000', NULL, (Select Top 1 OId from Logistics_Item where Kodu='" + item.StokKodu + "') , NULL, '21C72501-4624-4D9A-A3BC-5CCD26954944');  ";


                    sonuc = Axiom.UpdateDeleteSql(null, sql_urun);

                }



                
                string sorurun1 = "Select OId from Production_UrunAgaci where StokKodu like '" + item.StokKodu + "' and Parents_Id is null";

                DataTable dt_urun1 = Axiom.GetDataTable(null, sorurun1);

                string parentid1 = "NULL";
                if (dt_urun1.Rows.Count > 0)
                {
                    parentid1 = "'" + dt_urun1.Rows[0].ItemArray[0].ToString() + "'";
                }





                string soropr = "Select OId from Production_UrunAgaci  where StokKodu like '"+item.OperasyonKodu+"' and Parents_Id =  (Select top 1 OId from Production_UrunAgaci where StokKodu like '"+item.StokKodu+"' and Parents_Id is null)";


                DataTable dt_opr = Axiom.GetDataTable(null, soropr);

                string oprparentid = "NULL";
                if (dt_opr.Rows.Count > 0)
                {
                    oprparentid = "'" + dt_opr.Rows[0].ItemArray[0].ToString() + "'";
                }
                

                string sql_operasyon = " INSERT INTO [Production_UrunAgaci] ([OId], [Sira], [StokKodu], [UrunStatus], [Miktar], [IsSonRevizyon], [HazirlikSuresi], [OperasyonSuresi], [Parents_Id], ";
                sql_operasyon = sql_operasyon + "  [Item_Id], [MakinePersonelGrubu_Id], [Sirket_Id]) VALUES (NEWID(), 10, N'" + item.OperasyonKodu + "', 2, 0.000000, '1', '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000',   ";
                sql_operasyon = sql_operasyon + "  " + parentid1 + ", (Select Top 1 OId from Logistics_Item where Kodu='" + item.OperasyonKodu + "'), '4D36DC27-12EC-EE11-9EEB-B8AEED211524', '21C72501-4624-4D9A-A3BC-5CCD26954944');  ";
                sonuc = Axiom.UpdateDeleteSql(null, sql_operasyon);


                string soropr1 = "Select OId from Production_UrunAgaci  where StokKodu like '" + item.OperasyonKodu + "' and Parents_Id =  (Select top 1 OId from Production_UrunAgaci where StokKodu like '" + item.StokKodu + "' and Parents_Id is null)";


                DataTable dt_opr1 = Axiom.GetDataTable(null, soropr1);

                string oprparentid1 = "NULL";
                if (dt_opr1.Rows.Count > 0)
                {
                    oprparentid1 = "'" + dt_opr1.Rows[0].ItemArray[0].ToString() + "'";
                }

                if (string.IsNullOrEmpty(item.KullanilanKodu)== false)
                {
                    string sql_hammade = " INSERT INTO [Production_UrunAgaci] ([OId], [Sira], [StokKodu], [UrunStatus], [Miktar], [IsSonRevizyon], [HazirlikSuresi], [OperasyonSuresi], [Parents_Id], [Item_Id], ";
                    sql_hammade = sql_hammade + "   [MakinePersonelGrubu_Id], [Sirket_Id]) VALUES (NEWID(), 1, N'" + item.KullanilanKodu + "', 1, " + Convert.ToDecimal(item.KUllanilanMiktar).ToString().Replace(",",".") + ", '1', '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000', ";
                    sql_hammade = sql_hammade + " " + oprparentid1 + " , (Select Top 1 OId from Logistics_Item where Kodu='" + item.KullanilanKodu + "'), NULL, '21C72501-4624-4D9A-A3BC-5CCD26954944'); ";


                    sonuc = Axiom.UpdateDeleteSql(null, sql_hammade);
                }

           

            }

            MessageBox.Show("Bitti");
            


        }


        public static void ShowPriview(DataTable dataTable)
        {

            
            Preview pw = new Preview(DataTableToNode(dataTable),dataTable);

            pw.Show();

            
        }
     
     
        public static TreeView DataTableToNode(DataTable dataTable)
        {


            DataTable dt = dataTable;
            TreeView treeView1 = new TreeView();
            int i = 0;
            treeView1.Nodes.Clear();
            
            while (i < dt.Rows.Count)
            {
                DataRow row = dt.Rows[i];
                string product = row.ItemArray[0].ToString() + " - " + row.ItemArray[1].ToString();
                TreeNode productNode = treeView1.Nodes.Add(product);
                while (i < dt.Rows.Count && row.ItemArray[0].ToString() + " - " + row.ItemArray[1].ToString() == product)
                {
                    //string operasyon = row.Field<string>(8).ToString();
                    string operasyon = row.ItemArray[8].ToString() + " - " + row.ItemArray[9].ToString();
                    TreeNode operasyonNode = productNode.Nodes.Add(operasyon);
                    while (i < dt.Rows.Count && row.ItemArray[0].ToString() + " - " + row.ItemArray[1].ToString() == product && operasyon == row.ItemArray[8].ToString() + " - " + row.ItemArray[9].ToString())
                    {

                        string part = row.ItemArray[13].ToString() + " - " + row.ItemArray[14].ToString();
                        if (string.IsNullOrEmpty(row.ItemArray[13].ToString())==false)
                        {
                            operasyonNode.Nodes.Add(part);
                        }
                        
                        
                        if (++i < dt.Rows.Count)
                            row = dt.Rows[i];
                    }
                }
            }



            //TreeNode t = new TreeNode();
            return treeView1;









        }
        public static void CreateItems(BOM bom)
        {

            #region item oluşturma 
            

            string sql_item = "Select * from Logistics_Item where ItemType=1 and IsCihaz = 0 and IsKullaniliyor=1 and Kodu like '"+bom.StokKodu+"'";

            DataTable dt = Axiom.GetDataTable(null, sql_item);
            if (dt.Rows.Count==0)
            {
                string insert_item = "INSERT INTO [Logistics_Item] ([OId], [Kodu], [Adi], [adi2], [Adi3], [ItemType], [IsCihaz], [IsKullaniliyor], [StokTipleri], [ItemBirimTanimi1_Id], [ItemGroup_Id], [Sirket_Id]) VALUES ";
                insert_item = insert_item + " (NEWID(), N'"+bom.StokKodu+ "', N'" + bom.StokAdi + "', N'" + bom.Adi2 + "', N'" + bom.Adi3 + "', 1, '0', '1', 2, (Select top 1 OId from Logistics_ItemBirimTanimi where Kodu='" + bom.BOMBirim + "'), 'B605D66A-CCB4-4EC6-A22D-C0E65F5FE7D5', '21C72501-4624-4D9A-A3BC-5CCD26954944');  ";
                string sonuc_item = Axiom.UpdateDeleteSql(null, insert_item);
                if (sonuc_item != "Başarılı")
                {
                    MessageBox.Show(sonuc_item);
                }
            }


            #endregion


            #region Operasyon oluşturma 


            string sql_operation = "Select * from Logistics_Item where ItemType=3 and IsCihaz = 0 and IsKullaniliyor=1 and Kodu like '" + bom.OperasyonKodu + "'";

            DataTable dt_operation = Axiom.GetDataTable(null, sql_operation);
            if (dt_operation.Rows.Count == 0)
            {
                string insert_operation = "INSERT INTO [Logistics_Item] ([OId], [Kodu], [Adi], [adi2], [Adi3], [ItemType], [IsCihaz], [IsKullaniliyor], [StokTipleri], [ItemGroup_Id], [Sirket_Id]) VALUES ";
                insert_operation = insert_operation + " (NEWID(), N'" + bom.OperasyonKodu + "', N'" + bom.OperasyonAdi+ "', N'" + null + "', N'" + null + "', 3, '0', '1', 2,  'B605D66A-CCB4-4EC6-A22D-C0E65F5FE7D5', '21C72501-4624-4D9A-A3BC-5CCD26954944');  ";
                string sonuc_operation = Axiom.UpdateDeleteSql(null, insert_operation);
                if (sonuc_operation != "Başarılı")
                {
                    MessageBox.Show(sonuc_operation);
                }
            }
            #endregion
            #region hammade oluşturma 
            if (string.IsNullOrEmpty(bom.KullanilanKodu) != true)
            {

           
               
                string hamkod = "NULL";
            if (string.IsNullOrEmpty(bom.KullanilanKodu) != true)
            {
                hamkod = bom.KullanilanKodu;
            }
            string sql_ham = "Select * from Logistics_Item where ItemType=1 and IsCihaz = 0 and IsKullaniliyor=1 and Kodu like '" + hamkod + "'";

            DataTable dt_ham = Axiom.GetDataTable(null, sql_ham);
            if (dt_ham.Rows.Count == 0)
            {
                    string insert_ham = "INSERT INTO [Logistics_Item] ([OId], [Kodu], [Adi], [adi2], [Adi3], [ItemType], [IsCihaz], [IsKullaniliyor], [StokTipleri], [ItemBirimTanimi1_Id], [ItemGroup_Id], [Sirket_Id]) VALUES ";
                    insert_ham = insert_ham + " (NEWID(), N'" + bom.KullanilanKodu + "', N'" + bom.KullanilanAdi + "', N'" + bom.Adi2 + "', N'" + bom.Adi3 + "', 1, '0', '1', 2, (Select top 1 OId from Logistics_ItemBirimTanimi where Kodu='" + bom.BOMBirim + "'), 'B605D66A-CCB4-4EC6-A22D-C0E65F5FE7D5', '21C72501-4624-4D9A-A3BC-5CCD26954944');  ";
                    string sonuc_ham = Axiom.UpdateDeleteSql(null, insert_ham);
                if (sonuc_ham != "Başarılı")
                {
                    MessageBox.Show(sonuc_ham);
                }
            }
            }
            #endregion


            #region Lokasyon oluşturma 


            string makkod = string.IsNullOrEmpty(bom.MakinePersonelGrubuKodu) == true ? "yokyokyok" : bom.MakinePersonelGrubuKodu;

            string sql_location = "Select * from Production_MakinePersonelGrubu where Kodu  like '" + makkod + "'";

            DataTable dt_location = Axiom.GetDataTable(null, sql_location);
            if (makkod!= "yokyokyok")
            {
                if (dt_location.Rows.Count == 0)
                {
                    string insert_location = "INSERT INTO [Production_MakinePersonelGrubu] ([OId], [Kodu], [Adi]) VALUES (NEWID(), N'" + bom.MakinePersonelGrubuKodu + "', N'" + bom.MakinePersonelGrubuAdi + "');";

                    string sonuc_location = Axiom.UpdateDeleteSql(null, insert_location);
                    if (sonuc_location != "Başarılı")
                    {
                        MessageBox.Show(sonuc_location);
                    }
                }

            }




            #endregion


        }

     
     
    }
   
}
