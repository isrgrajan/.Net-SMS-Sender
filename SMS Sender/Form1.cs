//Get API Key from https://www.isrgrajan.com/sms-api/

using System;using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SMS_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            refreshLog();
            apimanager();
        }
            private void btn_send_Click(object sender, EventArgs e)
        {
            string mobile = tb_phone.Text;
            string sID = tb_key.Text.ToString();
            if(sID.Equals(""))
            {
                MessageBox.Show("SMS API Key is missing");
            } else { 

            string sms = RbSMS.Text;
                if (mobile.Equals(""))
                {
                    MessageBox.Show("Mobile number should not be empty");
                }
                else if (sms.Equals(""))
                {
                    MessageBox.Show("SMS Content should not be empty");
                }
                else
                {

                    Match m = Regex.Match(mobile, @"^[789]\d{9}$");
                    if (!m.Success)
                    {
                        MessageBox.Show(mobile + " is not a valid number");
                    }
                    else
                    {

                        try
                        {
                            btn_send.Enabled = false;

                            string strUrl = "https://www.isrgrajan.com//api/sms/project/handler.php?phone=" + mobile + "&sms=" + sms + "&sid="+sID;
                            WebRequest request = HttpWebRequest.Create(strUrl);
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Stream s = (Stream)response.GetResponseStream();
                            StreamReader readStream = new StreamReader(s);
                            string dataString = readStream.ReadToEnd();
                            response.Close();
                            s.Close();
                            readStream.Close();
                            string log = dataString.ToString();
                            log = log.Replace(System.Environment.NewLine, log);
                            MessageBox.Show("Message has been sent");
                            writeLog("Content:{Phone: " + mobile + "; SMS: " + sms + "} " + log);

                        }
                        catch (Exception MSG)
                        {
                            MessageBox.Show("Error: Failed to send the SMS. Check logs for detailed information");
                            var time = DateTime.Now;
                            string date = time.ToString("dd/MM/yyyy, HH:mm:ss");
                            writeLog(MSG.ToString());
                        }


                    }

                }
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            tb_phone.Text = "";
            RbSMS.Text = "";
            btn_send.Enabled = true;
        }

        private void refreshLog()
        {
            if (File.Exists("sms_log.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("sms_log.txt");
                rb_log.Text = sr.ReadToEnd();
                sr.Close();

            }
            else
            {
                rb_log.Text = "Hooray! Enjoy your day. There are no logs";
            }
        }

        private void writeLog(string logContents)
        {
            var time = DateTime.Now;
            string date = time.ToString("dd/MM/yyyy, HH:mm:ss");

            if (File.Exists("sms_log.txt"))
            {
                using (var tw = new StreamWriter("sms_log.txt", true))
                {
                    tw.WriteLine(date + " " + logContents);
                }
                refreshLog();
            }
            else
            {
                File.Create("sms_log.txt").Dispose();
                using (TextWriter tw = new StreamWriter("sms_log.txt"))
                {

                    tw.WriteLine(date + " " + logContents);
                }
                refreshLog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            apiWrite(tb_key.Text.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var ping = new System.Net.NetworkInformation.Ping();
                var result = ping.Send("www.isrgrajan.com");

                if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                    return;
                for (int i = 0; i <= 5; i++)
                {
                    rb_diag.AppendText(result.ToString() + "\n");
                }
            } catch(Exception ex)
            {
                string abc = ex.ToString();
                MessageBox.Show("No internet connected");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(
                "Rundll32.exe", "ndfapi,NdfRunDllDiagnoseIncident");
            }
            catch (Exception network)
            {
                MessageBox.Show("Error: Failed to start network diagnose");
                writeLog(network.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear system logs?", "Clear System logs", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists("sms_log.txt"))
                {
                    File.Create("sms_log.txt").Close();
                }
                refreshLog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rb_diag.Text = "";
        }

        private void RbSMS_TextChanged(object sender, EventArgs e)
        {
            Int32 smsSize = Convert.ToInt32(RbSMS.TextLength);
            sms_Counter.Text = "SMS Size: " + smsSize;
            if (smsSize > 150)
            {
                sms_Counter.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                sms_Counter.ForeColor = System.Drawing.Color.Black;
            }

            if (smsSize > 159)
            {
                btn_send.Enabled = false;
                sms_Counter.Text = "Word limit reached";
                sms_Counter.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                btn_send.Enabled = true;
            }
        }

        private void RbSMS_KeyDown(object sender, EventArgs e)
        {
            
        }

        private void apimanager()
        {
            if (File.Exists("API.txt"))
            {
                    System.IO.StreamReader sr = new System.IO.StreamReader("API.txt");
                    tb_key.Text = sr.ReadToEnd();
                    sr.Close();
            }
            else
            {
                //do nothing
            }
        }

        private void apiWrite(string key)
        {
            if (File.Exists("API.txt"))
            {
                File.Create("API.txt").Close();
                using (var tw = new StreamWriter("API.txt", true))
                {
                    tw.WriteLine(key);
                }
            }
            else
            {
                File.Create("API.txt").Dispose();
                using (TextWriter tw = new StreamWriter("API.txt"))
                {

                    tw.WriteLine(key);
                }
            }
        }
    }


}
