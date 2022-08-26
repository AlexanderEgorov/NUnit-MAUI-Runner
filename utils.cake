using System.Net;
using System.Net.Sockets;


public void RecieveXmlReport(string fileName) {
    var file = System.IO.File.Create(fileName);
    var ip = IPAddress.Parse("127.0.0.1");
    var listener = new TcpListener(ip, 13000);
            
    listener.Start();

    var socket = listener.AcceptSocket();
    var data = new byte[1024];
    int readedBytes = 1;
    
    while (readedBytes != 0) {
        readedBytes = socket.Receive(data);

        for (int i = 0; i < readedBytes; i++)
            file.WriteByte(data[i]);
    }

    file.Close();
    socket.Close();
    listener.Stop();
}

public string GetSimulatorUDID() {
    foreach(var deviceInfo in ListAppleSimulators()) {
        if (deviceInfo.Name.Equals("iPhone 11")) {
            return deviceInfo.UDID;
        }
    }
    return "";
}