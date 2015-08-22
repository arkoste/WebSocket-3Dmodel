This project is a simple test of the interaction between a webpage showing a 3D model, and a C# WinForm 
WebSocketServer application.
I've simulated the mousemove commands on the webpage, throught a Panel placed on the WinForm application.

Components i've used:

1) C# WinForm WebSocketServer (VS2010 with framework 4.5 on win 7, and Monodevelop 4.0.12).
2) Javascipt for the webSocket client side.
3) WebPage with 3D model and JavaScripts that control it (dumped from 
   http://solarkiosk.altervista.org/venere/venere2.html)

The test i've made runs correctly under win 7 and ubuntu 14.04 under the condition below:

1) To display the 3D model on win 7, i've used Chrome 44.0.2403.155 m from the command line
   (on my pc: C:\Program Files (x86)\Google\Chrome\Application\chrome.exe --allow-file-access-from-files [The complete path of the main webPage])
2) To display the 3D model on Ubuntu 14.04, i've used Firefox 35.0.1

I haven't tested others combinations.

Under Win 7 (sources code under /webSocketServer)

1) Run WebSocketServer.exe
2) From the command line type: C:\Program Files (x86)\Google\Chrome\Application\chrome.exe --allow-file-access-from-files [The complete path of the main webPage]
3) On the Chrome web page press CTRL+I and go to Console: you should see "connected..." message
4) If you try to move the mouse on the Panel of the WebSocketServer application with 
   the left button pressed, you should see the movement of the 3D model. The same for the mousewheel.
	 
Under Ubuntu 14.04 (sources code under /linux)

1) Run WebSocketServer.exe
2) From the terminal, type: firefox [The complete path of the main webPage]
3) On the Firefox web page press CTRL+I and go to Console: you should see "connected..." message
4) If you try to move the mouse on the Panel of the WebSocketServer application with 
   the left button pressed, you should see the movement of the 3D model. The same for the mousewheel.

