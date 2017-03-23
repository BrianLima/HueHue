/*
 Name:		Sketch1.ino
 Created:	3/19/2017 3:28:54 PM
 Author:	Brian
*/

int incomingByte = 0;

void setup() {
	Serial.begin(9600);
}

void loop() {

	// send data only when you receive data:
	if (Serial.available() > 0) {
		// read the incoming byte:
		incomingByte = Serial.read();

		// say what you got:
		Serial.print("I received: ");
		Serial.println(incomingByte, DEC);
	}
}

