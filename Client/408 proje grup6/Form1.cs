using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//Basak Amasya
//İsil Dereli
//Mehmet Sencer Yarali
//Meltem Derelli
//Nurbanu Yılmaz

//Client - Phase 2 and 3

namespace _408_proje_grup6
{
    public partial class Form1 : Form
    {
        string chosen_path = "";
        bool dwn_path_chosen = false;
        //global variables
        string fileContent = "";
        string filename = "";
        bool terminating = false;
        bool connected = false;
        FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        Socket serverSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void con_button_Click(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //opening socket
            string IP = ip_text.Text; //getting the IP
            string name = name_text.Text; //getting the name
            if (name == "" || name == " ") //checking the user name
            {
                rich_text.AppendText("User name cannot be empty.\n");
                return;
            }
            int portNum;
            if (Int32.TryParse(port_text.Text, out portNum))
            {
                try
                {
                    serverSocket.Connect(IP, portNum); //connecting to the IP and port number
                   
                    con_button.Enabled = false;
                    connected = true;
                    timer1.Enabled = true; 

                    rich_text.AppendText("Connection established...\n");
                    bro_button.Enabled = true;
                    Byte[] buffer_send = Encoding.Default.GetBytes(name);
                    serverSocket.Send(buffer_send); //sending the name to the server
                    rich_text.AppendText("User name: " + name + "\n");

                    Byte[] buffer = new Byte[64];
                    serverSocket.Receive(buffer); //whether connection is successful or username is not unique

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    rich_text.AppendText("Server: " + incomingMessage);

                    if (incomingMessage.Equals("Your name is not unique. Please check your username."))
                    {

                        try
                        {
                            Byte[] bufferexit = Encoding.Default.GetBytes("EXI");
                            serverSocket.Send(bufferexit);
                            serverSocket.Close();  //should close the socket because name is not unique
                        }
                        catch
                        {
                            serverSocket.Close();  //should close the socket because name is not unique
                        }
                        timer1.Enabled = false;
                        rich_text.AppendText("Connection is closed.\n");
                        con_button.Enabled = true; //can connect again
                        dis_button.Enabled = false; //cannot disconnect, browse or upload
                        bro_button.Enabled = false;
                        upload_button.Enabled = false;
                        button2.Enabled = false;
                        button_public.Enabled = false;


                    }
                    
                    else //name is unique, connection is established
                    {
                        con_button.Enabled = false;
                        dis_button.Enabled = true; //can disconnect and browse
                        bro_button.Enabled = true;
                        list_button.Enabled = true;
                        download_button.Enabled = true;
                        copy_button.Enabled = true;
                        delete_button.Enabled = true;
                        button2.Enabled = true;
                        button_public.Enabled = true;
                    }


                }
                catch
                {
                    rich_text.AppendText("Problem occurred while connecting...\n");                }
            }
            else
            {
                rich_text.AppendText("Check the port\n");

            }

        }

        private void bro_button_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) //taken from docs.microsoft
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt"; //showing only txt files
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                filename = openFileDialog.FileName; //getting the filename
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    upload_button.Enabled = true; //browsing completed, can upload now
                                                    //Get the path of specified file
                    filePath = openFileDialog.FileName; //getting the path of the file

                    var fileStream = openFileDialog.OpenFile(); // Read the contents of the file into a stream
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            char k = '\\';
            string str_dir = filePath;
            int num = (str_dir.Length - 1);
            while (num > -1)
            {
                if ((str_dir[num]) == k) //getting rid of the beginning of the path, only getting the filename
                {
                    string outputd = str_dir.Substring(num + 1, str_dir.Length - num - 5);
                    filename = outputd;
                    break;
                }
                num--;
            }
            if (!connected)
            {
                upload_button.Enabled = false;
            }
  
        }
        private void dis_button_Click(object sender, EventArgs e)
        {      
            terminating = true;
            connected = false;
            try
            {
                Byte[] bufferexit = Encoding.Default.GetBytes("EXI"); //lettin the server know that we are disconnecting
                serverSocket.Send(bufferexit);

                serverSocket.Close(); // closing the socket
            }
            catch
            {
                serverSocket.Close(); // closing the socket
            }

            upload_button.Enabled = false; //enabling and disabling appropriate buttons
            bro_button.Enabled = false;
            con_button.Enabled = true;
            dis_button.Enabled = false;
            rich_text.AppendText("Client has disconnected.\n"); //displaying that client has disconnected
            list_button.Enabled = false;
            download_button.Enabled = false;
            copy_button.Enabled = false;
            delete_button.Enabled = false;
            button2.Enabled = false;
            button_public.Enabled = false;
            timer1.Enabled = false;
        }

        private void upload_button_Click(object sender, EventArgs e) 
        {
            try
            {
                timer1.Enabled = false;
                Byte[] buffer = Encoding.Default.GetBytes("UPL"); //sending the upload command
                serverSocket.Send(buffer);

                string size = (fileContent.Length).ToString();

                Byte[] buffer1 = Encoding.Default.GetBytes(size);
                serverSocket.Send(buffer1); //sending the size of the file

                Byte[] bufferack = new Byte[16];
                serverSocket.Receive(bufferack); //receiving the acknowledgement
                Byte[] buffer2 = Encoding.Default.GetBytes(filename);
                serverSocket.Send(buffer2); //sending the filename

                Byte[] bufferack2 = new Byte[16];
                serverSocket.Receive(bufferack2); //receiving the acknowledgement
                Byte[] buffer3 = Encoding.Default.GetBytes(fileContent);
                
                if (buffer3.Length == 0)
                {
                    buffer3 = Encoding.Default.GetBytes("FileEmpty");
                    serverSocket.Send(buffer3);
                }
                else
                {
                    Byte[] buffer3x = Encoding.Default.GetBytes("FileContent");
                    serverSocket.Send(buffer3x);
                }

                Byte[] bufferack_content = new Byte[16];
                serverSocket.Receive(bufferack_content);

                if (!Encoding.Default.GetString(buffer3).Equals("FileEmpty"))
                {
                    serverSocket.Send(buffer3);
                }
                else
                {
                    serverSocket.Send(Encoding.Default.GetBytes("dummy"));
                }

                Byte[] bufferack3 = new Byte[64];
                serverSocket.Receive(bufferack3); //receiving the notification acknowledgement
                string notification = Encoding.Default.GetString(bufferack3);
                rich_text.AppendText("Server: " + notification); //displaying the notification

                upload_button.Enabled = false; // should browse again to upload
                timer1.Enabled = true;
            }
            catch //closing the connection when server is disconnected
            {
                if (!terminating)
                {
                    rich_text.AppendText("File is not send.\n");
               
                    ServerDisconnected();
           
                }

            }

        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            terminating = true;
            Environment.Exit(0);
        }

        private void list_button_Click(object sender, EventArgs e) //list button function
        {
            try
            {
                timer1.Enabled = false; //stoping the timer
                Byte[] buffer = Encoding.Default.GetBytes("LST");
                serverSocket.Send(buffer);//sending the command to server

                Byte[] bufferline = new Byte[2048];
                serverSocket.Receive(bufferline); //receiving the list sent by the server
                string line = Encoding.Default.GetString(bufferline);
                line = line.Substring(0, line.IndexOf("\0"));//removing zeros from the received list 

                string ack = "OK";
                Byte[] bufferack = Encoding.Default.GetBytes(ack);
                serverSocket.Send(bufferack);//sending an acknowledgement to the server

                if (line == "END") //if the received message from the server is END 
                {
                    rich_text.AppendText("You don't have any files.\n"); //no files uploaded by the current client
                }
                else
                {
                    rich_text.AppendText("Your file list is:\n");
                    while (line != "END") //showing the files uploaded by the current client until the server sends the end message which indicates that the files to be sent are done
                    {
                        rich_text.AppendText(line + "\n");//showing the received file from the server 
                        Byte[] bufferline2 = new Byte[2048];
                        serverSocket.Receive(bufferline2);//receiving another file which belongs to the same client 

                        Byte[] bufferack2 = Encoding.Default.GetBytes(ack);
                        serverSocket.Send(bufferack2);//sending an acknowledgement to the server

                        line = Encoding.Default.GetString(bufferline2);
                        line = line.Substring(0, line.IndexOf("\0"));//removing unnecessary zeros from the received file
                    }
                }
                timer1.Enabled = true;

            }

            catch (Exception ex)
            {
                ServerDisconnected();
            }

        }

        private void download_button_Click(object sender, EventArgs e) //download button function
        {

            try
            {
                if (!dwn_path_chosen) // bool value 'dwn_path_chosen' is false as default 
                {
                    DialogResult result = folderDlg.ShowDialog(); //creating a folder browse dialog for the user to select a folder to download the file on
                    folderDlg.ShowNewFolderButton = true;
                    rich_text.AppendText("Choose path for the files to be downloaded: " + "\n");

                    //showing the folder browsing dialog under the if statement
                    if (result == DialogResult.OK)
                    {
                        Environment.SpecialFolder root = folderDlg.RootFolder;
                        rich_text.AppendText("Path is chosen: " + folderDlg.SelectedPath + "\n");
                        chosen_path = folderDlg.SelectedPath;
                    }
                }

                string file_name = textBox_download.Text;
                string owner_name = textBox_owner.Text;
                if (file_name != "" && file_name != " " && owner_name != "" && owner_name != " ")//checking the file name and owner name not being empty 
                {
                    timer1.Enabled = false; //stoping the timer

                    Byte[] buffer = Encoding.Default.GetBytes("DWN");
                    serverSocket.Send(buffer);//sending the download command to the server 

                    Byte[] buffer2 = Encoding.Default.GetBytes(file_name);
                    serverSocket.Send(buffer2); //sending the filename

                    Byte[] bufferack14 = new Byte[16];
                    serverSocket.Receive(bufferack14); //receiving the acknowledgement from the server

                    Byte[] buffer15 = Encoding.Default.GetBytes(owner_name);
                    serverSocket.Send(buffer15); //sending the filename to the server

                    
                    Byte[] command = new Byte[16];
                    serverSocket.Receive(command);//receiving the YES/NO command from the server 

                    string commandy = Encoding.Default.GetString(command);
                    commandy = commandy.Substring(0, commandy.IndexOf("\0"));

                    if (commandy == "YES")//server granting access to the client to proceed with its request
                    {
                        string ack0 = "OK";
                        Byte[] bufferack0 = Encoding.Default.GetBytes(ack0);
                        serverSocket.Send(bufferack0);//sending acknowledgement to the server

                        Byte[] buffersize = new Byte[64];
                        serverSocket.Receive(buffersize); //receiving the filesize sent by the client from the server in order to download the file which was saved on server 

                        int filesize;
                        string filesizestring = Encoding.Default.GetString(buffersize);
                        int.TryParse(filesizestring, out filesize);

                        string ack = "OK";
                        Byte[] bufferack = Encoding.Default.GetBytes(ack);
                        serverSocket.Send(bufferack);//sending acknowledgement to the server


                        Byte[] bufferline2 = new Byte[filesize];
                        serverSocket.Receive(bufferline2);

                        string ack3yw = "OK";
                        Byte[] bufferack3yto = Encoding.Default.GetBytes(ack3yw);
                        serverSocket.Send(bufferack3yto); //sending acknowledgement to the server 

                        string read_this = Encoding.Default.GetString(bufferline2);

                        string path = System.IO.Path.Combine(chosen_path, file_name);
                        if (!File.Exists(path))//checking the existence of the database file in the selected path
                        {
                            var fileStream = new FileStream(path, FileMode.CreateNew);//Initializing a new instance of the FileStream with the saved file path and creation mode which opens the file.
                            fileStream.Close();
                            File.AppendAllText(path, read_this); //writing the file text

                            rich_text.AppendText(file_name + " is downloaded.\n");
                        }
                       else
                        {
                            //using the same counter implementation from the server in order to prevent downloaded files overwriting each other on the database

                            int count = 0;//setting the counter as 0 which indicates the number of times the same file was sent by the client

                            string tempsavedfile = path;
                            string tempsavedfile2 = path.Substring(0, file_name.LastIndexOf(".txt"));
                            string tempfilename = file_name.Substring(0, file_name.LastIndexOf(".txt"));

                            while (File.Exists(tempsavedfile)) //checking the existence of the sent file by the same client
                            {
                                count++; //incrementing the count by one if it already exists
                                tempsavedfile2 = path.Substring(0, path.LastIndexOf(".txt"));
                                tempfilename = file_name.Substring(0, file_name.LastIndexOf(".txt"));

                                if (count < 10)
                                {
                                    tempsavedfile2 = tempsavedfile2 + "-0" + count;
                                    tempfilename = tempfilename + "-0" + count;
                                }
                                else
                                {
                                    tempsavedfile2 = tempsavedfile2 + "-" + count;
                                    tempfilename = tempfilename + "-" + count;
                                }
                                tempsavedfile = tempsavedfile2 + ".txt";
                            }
                            string newpath = tempsavedfile2;
                            newpath = newpath + ".txt";
                            tempfilename = tempfilename + ".txt";

                            var fileStream = new FileStream(newpath, FileMode.CreateNew); //Initializing a new instance of the FileStream with the saved file path and creation mode which opens the file.
                            fileStream.Close();
                            File.AppendAllText(newpath, read_this); //writing the file text

                            rich_text.AppendText(file_name+ " owned by " + owner_name + " is downloaded as " + tempfilename + ".\n");

                        }

                    }
                    else if (commandy == "NO")//server not granting access to proceed
                    {
                        rich_text.AppendText("You dont have access to the file or file doesn't exist\n ");

                    }
                }

                else
                {
                    rich_text.AppendText("File name or owner name cannot be empty.\n");
                }

                if (connected)
                {
                    timer1.Enabled = true;
                }

            }
            catch (Exception exp)
            {
                ServerDisconnected();          
            }

        }

        private void delete_button_Click(object sender, EventArgs e) //delete button function
        {
            try
            {
                timer1.Enabled = false; //stoping the timer
                 
                if (textBox1.Text == "" || textBox1.Text == " ") //file to be deleted textbox cannot be empty
                {
                    rich_text.AppendText("File name cannot be empty.\n");//error message
                }
                else
                {
                    Byte[] buffer = Encoding.Default.GetBytes("DEL");
                    serverSocket.Send(buffer);//sending the delete command to the server

                    serverSocket.Send(Encoding.Default.GetBytes(textBox1.Text));//sending the file to be deleted to the server

                    Byte[] buffer_not = new Byte[64];
                    serverSocket.Receive(buffer_not);//receiving that the delete is done by the server

                    string notification = Encoding.Default.GetString(buffer_not);//assigning message sent by the server to string
                    notification = notification.Substring(0, notification.IndexOf("\0"));
                    rich_text.AppendText(notification + "\n");//printing out the message sent by the server that the deletion is done successfully 
                }
                
                timer1.Enabled = true; //enabling the timer back
            }
            catch
            {
                ServerDisconnected();
            }

        }

        private void copy_button_Click(object sender, EventArgs e)//copy button function
        {
            try
            {
                timer1.Enabled = false; //stoping the timer

                if (textBox_copy.Text == "" || textBox_copy.Text == " ") //checking the filename
                {
                    rich_text.AppendText("File name cannot be empty.\n");
                }
                else
                {
                    Byte[] buffer = Encoding.Default.GetBytes("COP");
                    serverSocket.Send(buffer);//sending the copy command to the server

                    serverSocket.Send(Encoding.Default.GetBytes(textBox_copy.Text));//sending the file name to copied to the server

                    Byte[] buffer_not = new Byte[64];
                    serverSocket.Receive(buffer_not);//receiving the copied file from the server

                    string notification = Encoding.Default.GetString(buffer_not);//assigning the received notification to string
                    notification = notification.Substring(0, notification.IndexOf("\0"));
                    rich_text.AppendText(notification + "\n");//showing the notification on the rich textbox
                }
                
                timer1.Enabled = true; //enabling the timer back
            }
            catch
            {
                ServerDisconnected();
            }
 

        }

        private void button2_Click(object sender, EventArgs e)//list public files button function
        {
            try
            {
                timer1.Enabled = false; //stoping the time
                Byte[] buffer = Encoding.Default.GetBytes("PLS");
                serverSocket.Send(buffer);//sending the 'list public files' command to the server

                Byte[] bufferline = new Byte[2048];
                serverSocket.Receive(bufferline);//receiving the public files list by the server
                string line = Encoding.Default.GetString(bufferline);
                line = line.Substring(0, line.IndexOf("\0"));//removing unnecessary zeros

                string ack = "OK";
                Byte[] bufferack = Encoding.Default.GetBytes(ack);
                serverSocket.Send(bufferack);//sending an acknowledgement to the server

                if (line == "END")//received message from the server
                {
                    rich_text.AppendText("There are no public files.\n");//unsuccessfull request
                }
                else
                {
                    rich_text.AppendText("Public file list is:\n");
                    while (line != "END") //until server sends the message of there is no more files to be sent
                    {
                        rich_text.AppendText(line + "\n");

                        Byte[] bufferline2 = new Byte[2048];
                        serverSocket.Receive(bufferline2);//receiving either end message or another public file from the server

                        Byte[] bufferack2 = Encoding.Default.GetBytes(ack);
                        serverSocket.Send(bufferack2);//sending an acknowledgement to the server

                        line = Encoding.Default.GetString(bufferline2);
                        line = line.Substring(0, line.IndexOf("\0"));//removing unnecessary zeros
                    }
                }
                timer1.Enabled = true; //enabling the timer back
            }
            catch
            {
                ServerDisconnected();
            }
        

        }

        private void button_public_Click(object sender, EventArgs e)//make public button function
        {
            try
            {
                timer1.Enabled = false; //stoping the timer       

                string filenamepublic = textBox_public.Text;
                if (filenamepublic == "" || filenamepublic == " ") //checking the filename
                {
                    rich_text.AppendText("File name cannot be empty.\n");
                }
                else
                {
                    Byte[] buffer = Encoding.Default.GetBytes("MPU");
                    serverSocket.Send(buffer);//sending make public command to the server

                    serverSocket.Send(Encoding.Default.GetBytes(filenamepublic));//sending the name of the file to be made public
                    Byte[] buffer_not = new Byte[64];
                    serverSocket.Receive(buffer_not);//receiving the sent file as a public file 
                    Console.WriteLine(buffer_not);
                    string notification = Encoding.Default.GetString(buffer_not); 
                    notification = notification.Substring(0, notification.IndexOf("\0"));

                    rich_text.AppendText(notification);//printing the message sent by the server
                }
               
                timer1.Enabled = true; //enabling the timer back
            }
            catch
            {
                ServerDisconnected();
            }

        }

        private void timer1_Tick(object sender, EventArgs e) //a function to check the connection with the server
        {
            try
            {
                timer1.Enabled = false;
                Byte[] buffer1 = Encoding.Default.GetBytes("LIV");
                serverSocket.Send(buffer1); //checking the connection with the server every 1 second by sending 

                Byte[] bufferack = new Byte[16];
                serverSocket.Receive(bufferack);//and receiving acknowledgements
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                ServerDisconnected();//once the acknowledgement from the server is not received, calling the function of server being disconnected
            }
        }

        private void ServerDisconnected()
        {
            rich_text.AppendText("The server has disconnected.\n");
            serverSocket.Close(); //closing the socket and making appropriate changes in the buttons when server disconnects
            bro_button.Enabled = false;
            dis_button.Enabled = false;
            con_button.Enabled = true;
            upload_button.Enabled = false;
            list_button.Enabled = false;
            download_button.Enabled = false;
            copy_button.Enabled = false;
            delete_button.Enabled = false;
            button2.Enabled = false;
            button_public.Enabled = false;
            timer1.Enabled = false;
            connected = false;
        }

    }
}
