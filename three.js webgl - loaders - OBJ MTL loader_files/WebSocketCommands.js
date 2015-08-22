	var ws;
	var host = 'ws://localhost:8181/websession';
	//alert("Connecting to " + host + " ...");
	try {
			ws = new WebSocket(host);
	} catch (err) {
			console.log(err);
	}
	ws.onopen = function () {
			console.log("connected...");
	};
	ws.onclose = function () {
			console.log("Socket closed!");
	};
	ws.onmessage = function (evt) {
			
			var received_msg = evt.data.split(";");
			var evt = document.createEvent("MouseEvents");
		
		  console.log('ricevuto: ' + received_msg[0] + 'X:' + received_msg[1] + 'Y:' + received_msg[2]);
			
			switch (received_msg[0]) {
			
				case "D":										
					evt.initMouseEvent("mousedown", true, true, window,1, 0, 0,received_msg[1], received_msg[2],false, false, false, false, 0, null);
					document.dispatchEvent(evt);

					break;
			
				case "M":
					evt.initMouseEvent("mousemove", true, true, window,1, 0, 0,received_msg[1], received_msg[2],false, false, false, false, 0, null);
					console.log("evtM.client:" + evt.clientX + "evt.clientY" + evt.clientY);
					document.dispatchEvent(evt);

					break;
					
				case "U":										
					evt.initMouseEvent("mouseup", true, true, window,1, 0, 0,0, 0,false, false, false, false, 0, null);
					document.dispatchEvent(evt);

					break;
					
				case "Z":	
				
					evt.initEvent('mousewheel', true, true);
					evt.wheelDelta = received_msg[1];
					
					document.dispatchEvent(evt);

					break;
				
			  default:
				  alert("comando non riconosciuto","erato comando");
			}
						
	};
