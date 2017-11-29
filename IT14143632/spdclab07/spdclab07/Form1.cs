using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Messaging;

namespace spdclab07
{
    public partial class Form1 : Form
    {

        MessageQueue mesageq = new MessageQueue();

        public Form1()
        {
            InitializeComponent();
            mesageq.Path = @".\private$\Bills2";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageQueue.Exists(mesageq.Path))
            {
                //Exists
                sendData2Queue();
            }
            else
            {
                // Creates the new queue named "Bills"
                MessageQueue.Create(mesageq.Path);
                sendData2Queue();
            }
        }

        private void sendData2Queue()
        {
            mesageq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            mesageq.ReceiveCompleted += billingQ_ReceiveCompleted;
            mesageq.Send(DateTime.Now.ToString());
            mesageq.Send("Hureaaa");
            mesageq.Send(DateTime.Now.ToString());
            // mesageq.Send("Hujghghghggreaaa");
            mesageq.BeginReceive();
            mesageq.Close();

        }

        void billingQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = mesageq.EndReceive(e.AsyncResult);
                string data = msg.Body.ToString();
                MessageBox.Show(DateTime.Now.ToString());
                // Process the logic be sending the message
                //Restart the asynchronous receive operation.
                mesageq.BeginReceive();
            }
            catch (MessageQueueException qexception){
               // MessageBox.Show("kfgjhbjkgh;fjkd;jkdfgh;fjkdgh;fjkdgh");
            }

        }
    }
}
