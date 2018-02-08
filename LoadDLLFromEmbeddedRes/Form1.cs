///<remarks>
///Load and use DLL from Embedded Resources 
///</remarks>
///<summary>
///Example of using Embedded Resource DLL
///
///"In an ordinary way, when we added a reference of a custom 
///built or third party DLL, it will be distributed along with 
///our main EXE application at the same folder. If the DLL is 
///not existed in the same folder with the EXE application, an 
///exception will be thrown. There are some cases that we wish 
///to pack / include the DLL within the EXE application. Therefore, 
///this article introduces a solution of loading DLLs from embedded 
///resources. "
///Reference:
/// https://www.codeproject.com/Articles/528178/Load-DLL-From-Embedded-Resource
///</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadDLLFromEmbeddedRes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            EmbeddedAssembly.Load("LoadDLLFromEmbeddedRes.Bunifu_UI_v1.5.3.Patched.dll", "Bunifu_UI_v1.5.3.Patched.dll");
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
