using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Task0
{
    public partial class MainForm : Form
    {
        private int _categoriesCount;
        private DataTable _dt = new DataTable();

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dt = new DataTable();
            _dt.Rows.Clear();

            using (var sr = new StreamReader("Data"))
            {
                var readLine = sr.ReadLine();
                if (readLine != null)
                {
                    var tableScheme = readLine.Split(',');
                    foreach (var columnName in tableScheme)
                        _dt.Columns.Add(columnName);
                }
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line != null) _dt.Rows.Add(line.Split(','));
                }
            }
            dataGridView.DataSource = _dt;


            var categories =
                _dt.Columns.Cast<DataColumn>()
                    .Select(clm => clm)
                    .Where(clm => clm.ColumnName.Contains("L"));
            _categoriesCount = categories.Count();

            var head = new Node("");
            var body = head;

            foreach (var cat in _dt.Rows.Cast<DataRow>().Select(row => (string) row[0]).Distinct())
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
                var propperties = _dt.Rows.Cast<DataRow>()
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
                _dt.Rows.Cast<DataRow>()
                    .Where(row => CompareRowWithNode(row, node)).Select(str => (string) str.ItemArray[lvl])
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
            var head = node;
            var compareList = new List<string>();
            while (head.Parent != null)
            {
                if (!string.IsNullOrEmpty(head.Name))
                    compareList.Add(head.Name);
                head = head.Parent;
            }
            compareList.Reverse();
            return !compareList.Where((string str, int i) => !((string) row.ItemArray[i]).Contains(str)).Any();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }
    }

    public class Node
    {
        public List<Node> ChildNodes = new List<Node>();
        public string Name;
        public Node Parent;
        public List<string> Properties;

        public Node(string name)
        {
            Name = name;
        }

        public TreeNode TransferToTreeNode()
        {
            var treeNode = new TreeNode(Name);

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