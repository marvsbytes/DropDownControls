using System;
using System.Data;
using System.Diagnostics;
using System.IO;

using DropDownControls;

namespace DemoApp
{

    public class DemoSearchBox : DropDownSearchBox {

		DataTable _table;

		public void EnsureTableLoaded() {
			if (_table == null) {
				_table = new DataTable();
				_table.PrimaryKey = new DataColumn[] { _table.Columns.Add("Word") };

				using (StreamReader sr = new StreamReader(File.OpenRead("WordList.txt"))) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						_table.Rows.Add(line.Trim());
					}
				}
			}
		}

		[DebuggerHidden]
		protected override void OnPerformSearch(PerformSearchEventArgs e) {
			string adoFilter = String.Format("Word LIKE '{0}%'", e.SearchTerm.Replace("'", "''"));
			foreach (DataRow dr in _table.Select(adoFilter)) {
				e.CancellationToken.ThrowIfCancellationRequested();
				e.Results.Add(new ComboTreeNode(dr.Field<string>(0)));
			}
		}
	}
}
