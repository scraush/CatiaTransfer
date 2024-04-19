using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatiaTransfer
{
    public partial class Preview : Form
    {
        TreeView tw = null;
        DataTable dataTablegelen = null;
        public Preview(TreeView treeView,DataTable dataTable)
        {
            InitializeComponent();
            tw = treeView;
            dataTablegelen = dataTable;

        }

        private void Preview_Load(object sender, EventArgs e)
        {

  
            foreach (TreeNode item in tw.Nodes)
            {
                



                    treeView1.Nodes.Add((TreeNode)item.Clone());


                item.EnsureVisible();
                
                
            }
            

            treeView1.Refresh();
            treeView1.CollapseAll();
            
            

        }



        private void Preview_Shown(object sender, EventArgs e)
        {
       
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<BOM> bomlist = new List<BOM>();
            foreach (DataRow dr in dataTablegelen.Rows)
            {
                BOM bm = new BOM();


                bm.StokKodu = dr.ItemArray[0].ToString();
                bm.StokAdi = dr.ItemArray[1].ToString();
                bm.Adi2 = dr.ItemArray[2].ToString();
                bm.Adi3 = dr.ItemArray[3].ToString();
                bm.StokTipi = Convert.ToInt32(dr.ItemArray[4]);
                bm.Revizyon = Convert.ToInt32(dr.ItemArray[5]);
                bm.Miktar = Convert.ToInt32(dr.ItemArray[6]);
                bm.BOMBirim = "Ad";
                bm.OperasyonSiraNo = Convert.ToInt32(dr.ItemArray[7]);
                bm.OperasyonKodu = dr.ItemArray[8].ToString();
                bm.OperasyonAdi = dr.ItemArray[9].ToString();
                //bm.HazirlikSuresi = dr.ItemArray[10].ToString();
                //bm.OperasyonSuresi = dr.ItemArray[11].ToString();
                //bm.MakinePersonelGrubuKodu = dr.ItemArray[00].ToString();
                //bm.MakinePersonelGrubuAdi = dr.ItemArray[00].ToString();
                bm.KullanilanSiraNo = Convert.ToInt32(dr.ItemArray[12]);
                bm.KullanilanKodu = dr.ItemArray[13].ToString();
                bm.KullanilanAdi = dr.ItemArray[14].ToString();
                //bm.KullanilanYedekTip = (int)dr.ItemArray[15];
                
                bm.KullanilanBirim = dr.ItemArray[16].ToString();
                bm.KUllanilanMiktar = Convert.ToDecimal(dr.ItemArray[17]);
                bm.BOMAciklama = "Aktarımdan Gelen";
                bm.KullanilanAciklama = "Aktarımdan Gelen";

                bomlist.Add(bm);

            }
       

            Transfer.CreateBom(bomlist);
            this.Close();


        }
    }
}
