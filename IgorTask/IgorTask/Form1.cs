using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace IgorTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();

        private void button1_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Rows.Clear();

            using (StreamReader sr = new StreamReader("Data"))
            {
                var tableScheme = sr.ReadLine().Split(',');
                foreach (var columnName in tableScheme)
                    dt.Columns.Add(columnName);
                while (!sr.EndOfStream)
                    dt.Rows.Add(sr.ReadLine().Split(','));
            }
            dataGridView.DataSource = dt;


            var categories =
                dt.Columns.Cast<DataColumn>()
                    .Select(clm => clm)
                    .Where(clm => clm.ColumnName.Contains("L"))
                    .Cast<DataColumn>();
            _categoriesCount = categories.Count();

            var head = new Node("");
            Node body = head;

            foreach (var cat in dt.Rows.Cast<DataRow>().Select(row => (string)row[0]).Distinct())
            {
                head.ChildNodes.Add(new Node(cat));
            }
            foreach (var childNode in head.ChildNodes)
            {
                childNode.Parent = head;
            }
            RunInNodes(body, 1);

            head.Name = "Transfered to TreeView";

            treeView.Nodes.Add(head.TransferToTreeNode());

        }

        private int _categoriesCount;
        private void RunInNodes(Node node, int lvl)
        {
            if (node.ChildNodes.Count == 0 || lvl >= _categoriesCount)
            {
                AddPropertiesToNode(node);
                return;

            }


            foreach (var childNode in node.ChildNodes)
            {
                AddChildsToNode(childNode, lvl);
                RunInNodes(childNode, lvl + 1);
            }
        }

        private void AddPropertiesToNode(Node node)
        {

            foreach (var childNode in node.ChildNodes)
            {
                childNode.Properties = new List<string>();
                var propperties = dt.Rows.Cast<DataRow>()
                    .First(row => CompareRowWithNode(row, childNode))
                    .ItemArray.Cast<string>().Where(((s, i) => i > _categoriesCount - 1));

                foreach (var propperty in propperties)
                {
                    childNode.Properties.Add(propperty);
                }

            }



        }

        private void AddChildsToNode(Node node, int lvl)
        {

            var childNodeNameCollection =
                dt.Rows.Cast<DataRow>()
                    .Where(row => CompareRowWithNode(row, node)).Select(str => (string)str.ItemArray[lvl])
                                    .Distinct();

            foreach (var childNodeName in childNodeNameCollection)
            {
                node.ChildNodes.Add(new Node(childNodeName));
            }
            foreach (var childNode in node.ChildNodes)
            {
                childNode.Parent = node;
            }

        }

        private bool CompareRowWithNode(DataRow row, Node node)
        {
            Node head = node;
            List<string> compareList = new List<string>();
            while (head.Parent != null)
            {
                if (!string.IsNullOrEmpty(head.Name))
                    compareList.Add(head.Name);
                head = head.Parent;
            }
            compareList.Reverse();
            return !compareList.Where((t, i) => !((string)row.ItemArray[i]).Contains(t)).Any();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }

    public class Node
    {
        public string Name;
        public List<Node> ChildNodes = new List<Node>();
        public Node Parent = null;
        public List<String> Properties = null;


        public Node(string name)
        {
            Name = name;
        }

        public TreeNode TransferToTreeNode()
        {
            TreeNode treeNode = new TreeNode(Name);

            foreach (var node in ChildNodes)
            {
                treeNode.Nodes.Add(node.TransferToTreeNode());
            }
            if (Properties != null)
                foreach (var property in Properties)
                    treeNode.Nodes.Add(property);


            return treeNode;
        }

        public static explicit operator TreeNode(Node node)
        {
            return new TreeNode(node.Name);
        }
    }







}
