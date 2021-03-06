using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//Basak Amasya
//İsil Dereli
//Mehmet Sencer Yarali
//Meltem Derelli
//Nurbanu Yılmaz

//Server - Phase 2 and 3

namespace project_server
{
    public partial class Form1 : Form
    {
        //global variables we will be using throughout the code:

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>(); //a list to keep the client sockets

        List<string> clientnames = new List<string>(); //created a list to store the client names as strings

        bool terminating = false;
        bool listening = false;

        FolderBrowserDialog folderDlg = new FolderBrowserDialog(); //path which will be choosen
        string DBpath = "";

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void listen_button_Click(object sender, EventArgs e)
        {

            int serverPort;

            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(50); //the server can queue up to 50 clients

                listening = true;
                listen_button.Enabled = false;

                Thread acceptThread = new Thread(Accept); 
                acceptThread.Start(); //start accepting

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }
        private void Accept()
        {
            while (listening)
            {
                try
                {

                    Socket newClient = serverSocket.Accept();
                    Byte[] receivingname = new Byte[64];
                    newClient.Receive(receivingname);//receiving the username by the client

                    string currentname = Encoding.Default.GetString(receivingname);//assigning the username to a string 
                    currentname = currentname.Substring(0, currentname.IndexOf("\0"));
                    bool check_return = false;//a bool variable to check the uniqueness of the username for the if statement to return
                    foreach (string clientname in clientnames)
                    {

                        if (clientname.Equals(currentname))//if clientname is found
                        {
                            logs.AppendText("Already existing client name.\n");
                            string warning = "Your name is not unique. Please check your username.";
                            Byte[] buffer = Encoding.Default.GetBytes(warning);
                            newClient.Send(buffer); //letting the client know
                            newClient.Close();//closing the socket
                            check_return = true; //setting the bool check return as true so that the client won't be able to progress until his/her username is unique

                        }
                    }

                    if (!check_return)//connecting the username once its name is unique
                    {
                        clientSockets.Add(newClient);//adding name to the socket client list
                        clientnames.Add(currentname);//adding name to the string client list
                        logs.AppendText(currentname + " is connected.\n");
                        string acceptmessage = "You are connected to the server.\n";
                        Byte[] buffer1 = Encoding.Default.GetBytes(acceptmessage);
                        newClient.Send(buffer1);//sending an informative message to the client stating that s/he is connected
                        Thread receiveThread = new Thread(() => Receive(newClient, currentname));
                        receiveThread.Start();
                    }

                }
                catch
                {
                    if (terminating)
                    {

                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }
        private void Receive(Socket thisClient, string currentname)
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog(); //chosing the path where the server will do the saving on, taken from https://www.c-sharpcorner.com/
                    saveFileDialog1.InitialDirectory = folderDlg.SelectedPath;

                    Byte[] buffercom = new Byte[4]; //initial command from the client
                    thisClient.Receive(buffercom);
                    string command = Encoding.Default.GetString(buffercom);
                    command = command.Substring(0, command.IndexOf("\0"));

                    if (command == "UPL") //copy command from the client
                    {

                        Byte[] buffer1 = new Byte[64];
                        thisClient.Receive(buffer1); //receiving the filesize sent by the client

                        int filesize;
                        string filesizestring = Encoding.Default.GetString(buffer1);
                        int.TryParse(filesizestring, out filesize);

                        string ack = "OK";
                        Byte[] bufferack = Encoding.Default.GetBytes(ack);
                        thisClient.Send(bufferack);//sending an acknowledgement to the client

                        Byte[] buffer2 = new Byte[64];
                        thisClient.Receive(buffer2); //receiving the filename sent by the client
                        string filename = Encoding.Default.GetString(buffer2);


                        char[] charlist = filename.ToCharArray();

                        if (charlist[0] == '\0') //checking if filename is valid, not empty
                        {
                            throw new System.InvalidOperationException("Logfile cannot be read-only");
                        }

                        string ack2 = "OK";
                        Byte[] bufferack2 = Encoding.Default.GetBytes(ack2);
                        thisClient.Send(bufferack2); //sending an acknowledgement to the client

                        string newfilename = currentname + filename;
                        newfilename = newfilename.Substring(0, newfilename.IndexOf("\0"));
                        filename = filename.Substring(0, filename.IndexOf("\0"));
                        string savedfile = System.IO.Path.Combine(@folderDlg.SelectedPath, newfilename); //adding the filename to the path

                        int count = 0;//setting the counter as 0 which indicates the number of times the same file was sent by the client

                        string tempsavedfile = savedfile + ".txt";
                        string tempsavedfile2 = savedfile;
                        string tempfilename = filename;

                        while (File.Exists(tempsavedfile)) //checking the existence of the sent file by the same client
                        {
                            count++; //incrementing the count by one if it already exists
                            tempsavedfile2 = savedfile;
                            tempfilename = filename;
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
                        savedfile = tempsavedfile2;
                        savedfile = savedfile + ".txt";
                        tempfilename = tempfilename + ".txt";


                        Byte[] buffer3x = new Byte[16];
                        thisClient.Receive(buffer3x);

                        string ack3 = "OK";
                        Byte[] bufferack3 = Encoding.Default.GetBytes(ack3);
                        thisClient.Send(bufferack3); //sending an acknowledgement to the client


                        string filestring = "";
                        string content_check = Encoding.Default.GetString(buffer3x).Trim('\0');
                        if (!content_check.Equals("FileEmpty"))
                        {
                            Byte[] buffer3 = new Byte[filesize]; //setting the byte of the file as big as the file itself
                            thisClient.Receive(buffer3); //receiving the file sent by the client by the size of the file which the server has taken previously
                            filestring = Encoding.Default.GetString(buffer3).Trim('\0');

                        }
                        else
                        {
                            Byte[] buffer3 = new Byte[filesize];
                            thisClient.Receive(buffer3);
                        }

                        var fileStream = new FileStream(savedfile, FileMode.CreateNew);//Initializing a new instance of the FileStream with the saved file path and creation mode which opens the file.
                        fileStream.Close();
                        File.AppendAllText(savedfile, filestring); //writing the file text

                        string notification = "Received and saved\n";
                        Byte[] buffer4 = Encoding.Default.GetBytes(notification);
                        thisClient.Send(buffer4); //sending an informative message to the client stating that the server has saved the sent file to its DataBase

                        DateTime now = DateTime.Now; //getting the upload time
                        string datetime = now.ToString("F");

                        filesizestring = filesizestring.Substring(0, filesizestring.IndexOf("\0"));
                        File.AppendAllText(DBpath, currentname + " " + tempfilename + " " + filesizestring + " " + datetime + " PRIV" + "\n"); //adding the file to the database
                        logs.AppendText("File named " + savedfile + " by " + currentname + " is saved" + "\n");
                    }
                    else if (command == "LST") // list command from the client
                    {

                        var lines = File.ReadLines(DBpath); 

                        foreach (var line in lines) //reading the lines from the Database which belongs to the client in a loop
                        {
                            string name = line.Substring(0, line.IndexOf(' ')); 

                            if (name == currentname) //selecting the files belonging to the client
                            {
                                string sending = line.Substring(line.IndexOf(' ') + 1, line.Length - (currentname.Length + 6)); //setting another string value as sending which only has the filename without the client's name and path
                                Byte[] buffersend = Encoding.Default.GetBytes(sending);
                                thisClient.Send(buffersend); //sending the files as a list to the client

                                Byte[] bufferack = new Byte[16];
                                thisClient.Receive(bufferack); //receiving the acknowledgement

                            }
                        }

                        Byte[] bufferend = Encoding.Default.GetBytes("END"); //informing that the list is sent to the client
                        thisClient.Send(bufferend);
                        logs.AppendText("List is send to " + currentname + "\n"); //showing that sending the requested list by the client successfully done

                    }
                    else if (command == "DWN") //getting the download command from the client
                    {
                        bool not_found = true; //setting a bool variable which checks the existence of the file requested to be downloaded by the client

                        Byte[] buffer_file = new Byte[2048];
                        thisClient.Receive(buffer_file); //receiving the file name to be opened
                        
                        Byte[] bufferok = Encoding.Default.GetBytes("OK");
                        thisClient.Send(bufferok); //sending the acknowledgement 

                        Byte[] buffer_owner = new Byte[2048];
                        thisClient.Receive(buffer_owner); //receiving the file name to be opened
                        
                        string fileename = Encoding.Default.GetString(buffer_file); // get the file name
                        string ownername = Encoding.Default.GetString(buffer_owner); // get the owner name of the file

                        fileename = fileename.Substring(0, fileename.IndexOf("\0")); // removing the zeros from the filename received from the client
                        ownername = ownername.Substring(0, ownername.IndexOf("\0")); //removing the zeros from the owner name received from the client

                        var lines = File.ReadLines(DBpath);

                        foreach (var line in lines) //reading the lines from the Database which the client requested in a loop
                        {
                            string name = line.Substring(0, line.IndexOf(' ')); 

                            string rest_of_line = line.Substring(line.IndexOf(' ') + 1);
                            string file_namei = rest_of_line.Substring(0, rest_of_line.IndexOf(' '));//getting only the name of the file from the database by using substring

                            if ((ownername == name && name == currentname && fileename == file_namei) || (ownername == name && fileename == file_namei && line.EndsWith("PUBL"))) //checking the compatibility of the ownername and current name and file name or the compatibility of the owner name, file name and the file being public in the database 
                            {
                                string accept = "YES";
                                Byte[] bufferacp = Encoding.Default.GetBytes(accept);
                                thisClient.Send(bufferacp); //informing the client that the server their requested is taken successfully
 
                                Byte[] bufferack0 = new Byte[16];
                                thisClient.Receive(bufferack0); //receiving the acknowledgement

                                string directoryy = DBpath.Substring(0, DBpath.Length - 6); //removing the db.txt part
                                string file_fullname = directoryy + name + fileename;

                                string content_text = "";
                                using (StreamReader streamReader = File.OpenText(file_fullname))
                                {
                                    content_text = streamReader.ReadToEnd(); //reading the whole file in order to get its size
                                }

                                string size = (content_text.Length).ToString(); //getting the size of the file 
                                Byte[] buffersize = Encoding.Default.GetBytes(size);
                                thisClient.Send(buffersize); //sending the size to the client

                                Byte[] bufferack = new Byte[16];
                                thisClient.Receive(bufferack); //receiving the acknowledgement

                                Byte[] buffersend1 = Encoding.Default.GetBytes(content_text);
                                thisClient.Send(buffersend1); //content is sent to the client

                                Byte[] bufferackfinal = new Byte[16];
                                thisClient.Receive(bufferackfinal); //receiving the acknowledgement
                                not_found = false;

                                logs.AppendText(file_namei + " owned by " + ownername + " downloaded by client " + currentname + "\n"); //showing that the requested download by the client is done successfully
                                break;
                            }
                             
                        }

                        if (not_found) //the case of the requested file not existing
                        {
                            string accepty = "NO";
                            Byte[] bufferacpy = Encoding.Default.GetBytes(accepty);
                            thisClient.Send(bufferacpy); //informing the client that the file cannot be downloaded
                            logs.AppendText("Requested file cannot be downloaded by client " + currentname + ".\n"); //showing that sending the client could not download the requested file on server
                        }

                    }
                    else if (command == "COP") //copy command from the client
                    {
                        Byte[] buffer_filename = new Byte[64];
                        thisClient.Receive(buffer_filename); //receiving the filename to copy from the client

                        string filename_to_copy = Encoding.Default.GetString(buffer_filename).Trim('\0'); //getting rid of the extra zeros

                        string copied_content = "";

                        string directoryy = DBpath.Substring(0, DBpath.Length - 6);//removing the db.txt part
                        string path = directoryy + currentname + filename_to_copy;

                        if (!File.Exists(path)) //the case of requested file not existing
                        {
                            logs.AppendText("Client requested a copy of " + filename_to_copy + " which does not exist.\n");//showing that the requested copy by the client is done successfully
                            thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_copy + " cannot be copied."));//informing the client about the unsuccessfull request
                        }

                        else
                        {

                            using (StreamReader streamReader = File.OpenText(path))
                            {
                                copied_content = streamReader.ReadToEnd();
                            }
                           
                            string tempsavedfile = path;
                            string tempsavedfile2 = path.Substring(0, filename_to_copy.LastIndexOf(".txt"));
                            string tempfilename = filename_to_copy.Substring(0, filename_to_copy.LastIndexOf(".txt"));

                            int count = 0;//used the same counter implementation starting from line 181

                            while (File.Exists(tempsavedfile)) //checking the existence of the sent file by the same client
                            {
                                count++; //incrementing the count by one if it already exists
                                tempsavedfile2 = path.Substring(0, path.LastIndexOf(".txt"));
                                tempfilename = filename_to_copy.Substring(0, filename_to_copy.LastIndexOf(".txt"));

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
                            newpath = newpath + ".txt"; //updated path
                            tempfilename = tempfilename + ".txt"; //updated filename 

                            DateTime now = DateTime.Now;
                            string datetime = now.ToString("F"); //getting the new upload time to use when creating a new database entry
                   
                            int filesizestring = copied_content.Length;

                            var lines = File.ReadAllLines(DBpath);
                            string found = "";

                            foreach (var line in lines) //used the same foreach implementation as line 297 
                            {
                                string name = line.Substring(0, line.IndexOf(' '));
                                string rest_of_line = line.Substring(line.IndexOf(' ') + 1);
                                string file_namei = rest_of_line.Substring(0, rest_of_line.IndexOf(' '));
                                
                                if (name == currentname && filename_to_copy == file_namei) //finding the requested file to be copied by the client in the database 
                                {
                                    if (line.EndsWith("PRIV")) // the file being a private file 
                                    {
                                        found = "PRIV"; //setting the found as PRIV in order to both save it as a private file and show it being private when sending a message to the client 
                                       
                                    }
                                    else
                                    {
                                        found = "PUBL";//setting the found as PUBL in order to both save it as a public file and show it being public when sending a message to the client
                                        
                                    }
                                    break;
                                }
                            }
                            if (found == "") // file not being private nor public 
                            {
                                logs.AppendText("Client requested a copy of " + filename_to_copy + " which does not exist.\n"); //showing the unsuccessfull request by the client
                                thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_copy + " cannot be copied.")); //sending an error message to the client
                                
                            }
                            else
                            {
                                var fileStream = new FileStream(newpath, FileMode.CreateNew);//Initializing a new instance of the FileStream with the saved file path and creation mode which opens the file.
                                fileStream.Close();
                                File.AppendAllText(newpath, copied_content); //writing the file text

                                File.AppendAllText(DBpath, currentname + " " + tempfilename + " " + filesizestring + " " + datetime + " " + found + "\n"); //adding the file to the database
                                logs.AppendText("File named " + tempfilename + " by " + currentname + " is saved as " + (found == "PRIV" ? "private" : "public") + "\n");

                                thisClient.Send(Encoding.Default.GetBytes(tempfilename + " copied successfully.")); // Send ack
                            }
                        }
                    }
                    else if (command == "DEL") //getting the download command from the client
                    {
                        Byte[] buffer_filename = new Byte[64];
                        thisClient.Receive(buffer_filename);//getting the file name to be deleted by the client
                        string filename_to_delete = Encoding.Default.GetString(buffer_filename).Trim('\0'); //removing the unnecessary zeros

                        string directoryy = DBpath.Substring(0, DBpath.Length - 6);
                        string path = directoryy + currentname + filename_to_delete;

                        if (!File.Exists(path)) //selected file not existing in the database 
                        {
                            logs.AppendText(filename_to_delete + " cannot be deleted.\n");
                            thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_delete + " cannot be deleted."));
                        }
                        else //selected file existing in the database 
                        {
                            using (var fileStream = File.OpenRead(DBpath));
                            var lines = File.ReadLines(DBpath); //getting the lines in the databse 
                            List<string> linestokeep = new List<string>(); //creating a list which will have the filnames after the deletion of the asked file
                            bool found = false; //a bool variable which will used to find the asked file to be deleted by the client

                            foreach (var line in lines)//a foreach statement to check every line in the database 
                            {
                                string name = line.Substring(0, line.IndexOf(' '));
                                string rest_of_line = line.Substring(line.IndexOf(' ') + 1);
                                string file_namei = rest_of_line.Substring(0, rest_of_line.IndexOf(' '));
                                if (name != currentname || filename_to_delete != file_namei) //checking the file to be deleted in the database, if the file is not the file to be deleted
                                {
                                    linestokeep.Add(line);//adding that file on the database to the previously created linestokeep list
                                }
                                else
                                {
                                    found = true;//if it's the requested file to be deleted, then setting the bool variable as true and not adding it to the list
                                }

                            }
                            if (found)//when the file to be deleted is found in the database
                            {
                                System.IO.File.Delete(path); //deleting the file to be deleted from the database 
                                File.WriteAllLines(DBpath, linestokeep); //pasting the linestokeep list on database since it has stored every file except the deleted file 

                                logs.AppendText(filename_to_delete + " is deleted.\n");//showing on the server that the file is deleted
                                thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_delete + " is deleted."));//informing the client about the deletion

                            }
                            else
                            {
                                logs.AppendText(filename_to_delete + " cannot be deleted.\n");//showing the unsuccessfull request made by the client on server
                                thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_delete + " cannot be deleted."));//informing the client about the unsuccessfull request
            
                            }
                        }
                    }
                    else if (command == "MPU") //command of making a file public by the client
                    {
                        Byte[] buffer_filename = new Byte[1024];
                        thisClient.Receive(buffer_filename);

                        string filename_to_public = Encoding.Default.GetString(buffer_filename).Trim('\0');
     
                        var lines = File.ReadLines(DBpath);
                        List<string> linestokeep = new List<string>();//creating a list which will have the filnames after making a file public, implemented the same way and purpose as the list on line 473
                        bool found = false;//a bool variable which will used to find the asked file to be made public by the client
                        bool madepublic = false;

                        foreach (var line in lines)
                        {
                            string newline = line;
                            string name = line.Substring(0, line.IndexOf(' '));
                            string rest_of_line = line.Substring(line.IndexOf(' ') + 1);
                            string file_namei = rest_of_line.Substring(0, rest_of_line.IndexOf(' '));

                            if (name == currentname && filename_to_public == file_namei)//checking and trying to find the file will be turned to public in database 
                            {
                                found = true;//setting the bool variable as true once the asked file is found
                                if (line.EndsWith("PRIV"))//checking the file's private/public situation
                                {
                                    newline = line.Substring(0, line.Length - 4);//removing the last 4 digits of the line since it is the part which stats the file being private
                                    newline = newline + "PUBL";//stating that this file is now a public file 
                                    madepublic = true;
                                }
                                else // the case of selected file already being a public one
                                {
                                    logs.AppendText(filename_to_public + " is already public.\n");//showing that the selected file is already a public file on server
                                    thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_public + " is already public.\n"));//informing the client about the file already a public file 
                                }
                                //break;

                            }

                            linestokeep.Add(newline); //adding the changed file as public to the previosuly created list 

                        }
                        if (found && madepublic) //corresponding file is found
                        {
                            File.WriteAllLines(DBpath, linestokeep);//posting the updated state of the files in the database 
                            logs.AppendText(filename_to_public + " is made public.\n");
                            thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_public + " is made public.\n"));

                        }
                        else if(!found)
                        {
                            logs.AppendText(filename_to_public + " cannot be made public by " + currentname + " or doesn't exist.\n");//unsuccessfull request

                            thisClient.Send(Encoding.Default.GetBytes("File named " + filename_to_public + " cannot be made public or doesn't exist.\n"));//unsuccessfull request

                        }

                    }
                    else if (command == "PLS") //command of listing public files by the client
                    {
                        var lines = File.ReadLines(DBpath);
                        foreach (var line in lines)
                        {
                            if (line.EndsWith("PUBL"))//getting every file which are public
                            {
                                string sending = line.Substring(0, line.Length - 5);
                                Byte[] buffersend = Encoding.Default.GetBytes(sending);//sending the asked list to the client
                                thisClient.Send(buffersend);

                                Byte[] bufferack = new Byte[16];
                                thisClient.Receive(bufferack); //receiving the acknowledgement

                            }
                        }
                        Byte[] bufferend = Encoding.Default.GetBytes("END"); //list has ended
                        thisClient.Send(bufferend);
                        logs.AppendText("Public files list is send to " + currentname + "\n");//request is successfully done 

                    }

                    else if (command == "EXI") //client is disconnecting
                    {
                        logs.AppendText(currentname + " has disconnected\n");
                        thisClient.Close();
                        clientSockets.Remove(thisClient); //removing the client from socket list, so it can connect with the same name again after disconnecting
                        clientnames.Remove(currentname); //removing the client from socket list, so it can connect with the same name again after disconnecting
                        connected = false;
                    }
                    else if (command == "LIV") //client is checking
                    {
                        Byte[] buffersend = Encoding.Default.GetBytes("LIV");
                        thisClient.Send(buffersend); //server is still alive
                    }

                }

                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText(currentname + " has disconnected\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient); //removing the client from socket list, so it can connect with the same name again after disconnecting
                    clientnames.Remove(currentname); //removing the client from socket list, so it can connect with the same name again after disconnecting
                    connected = false;
                }
            }
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;

            foreach (Socket socket in clientSockets)
            {
                socket.Close(); //closing each socket
            }
            serverSocket.Close();

            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e) //the choose path button's function, taken from https://www.c-sharpcorner.com/
        {
            DialogResult result = folderDlg.ShowDialog(); //creating a folder browse dialog for the user to select a folder
            folderDlg.ShowNewFolderButton = true;

            //showing the folder browsing dialog under the if statement
            if (result == DialogResult.OK)
            {
                Environment.SpecialFolder root = folderDlg.RootFolder;
                logs.AppendText("Path is choosen: " + folderDlg.SelectedPath + "\n");
                button1.Enabled = false;
                listen_button.Enabled = true;
                DBpath = System.IO.Path.Combine(@folderDlg.SelectedPath, "DB.txt"); //combinig the selecrted path with the DB.txt and storing it in the Database.

                if (!File.Exists(DBpath))//checking the existence of the database file in the selected path
                {
                    var fileStream = new FileStream(DBpath, FileMode.CreateNew);//if it does not exist, creating a new database file in the selected path
                    fileStream.Close();
                }
            }
        }       
       
    }
}
