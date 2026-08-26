namespace libHoneyBadger;

public class HoneyBadger{
	byte[] ddscomp = {0x07, 0x10, 0x00, 0x00};
	byte[] d5comp = new byte[108];
	byte[] nocomp = new byte[108];
	
	public bool DDSFixHeader(string[] ddsList, bool dxt5){
		d5comp[56] = 0x20; d5comp[60] = 0x04; d5comp[64] = 0x44; d5comp[65] = 0x58; 
		d5comp[66] = 0x54; d5comp[67] = 0x35; d5comp[89] = 0x10;
		nocomp[56] = 0x20; nocomp[60] = 0x41; nocomp[68] = 0x20; nocomp[74] = 0xFF; nocomp[77] = 0xFF;
		nocomp[80] = 0xFF; nocomp[87] = 0xFF; nocomp[88] = 0x02; nocomp[89] = 0x10;
		bool valid = true;

		foreach (string dds in ddsList){
			using (FileStream fs = File.Open(dds, FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite)){
				byte[] headerbytes = new byte[3];
				fs.Read(headerbytes, 0, 3);
				string header = System.Text.Encoding.UTF8.GetString(headerbytes, 0, 3);
				if (header != "DDS"){
					valid = true;
					continue;
				}
				fs.Seek(8, SeekOrigin.Begin);
				fs.Write(ddscomp);
				if (dxt5){
					fs.Seek(20, SeekOrigin.Begin);
					fs.Write(d5comp);
				}
				else{
					fs.Seek(20, SeekOrigin.Begin);
					fs.Write(nocomp);
				}
			}	
		}
		return valid; // returns false if any invalid headers were found
	}
}
