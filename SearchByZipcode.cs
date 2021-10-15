void Main()
{
	/*
	  Josh/Valerie -
	  
	  I wrote this code in LinqPad, which provides a few helper extensions like the ".Dump()" method,
	  which is about equivalent to a Console.WriteLine() in normal C#.  If you see that below, that's why.
	  
	  I've added a few comments to make clearer what I was doing.
	
	*/
	
	
	// using Newtonsoft.Json
	string jsonReps = System.IO.File.ReadAllText(@"D:\Temp\Swayable\find-my-rep-tom-main\data\legislators.json");
	Representatives = JsonConvert.DeserializeObject<List<Representative>>(jsonReps);
	
	// Confirming we've successfully gathered and deserialized the json.
	var rep = Representatives[0];
	rep.currentTerm.Dump("term");

	// Checking some assumptions: that all reps in the data HAVE a currentTerm
	Representatives.Any(r => r.currentTerm == null).Dump();
	Representatives.All(r => r.currentTerm.end > new DateTime(2019, 1, 1)).Dump();

	string jsonZips = System.IO.File.ReadAllText(@"D:\Temp\Swayable\find-my-rep-tom-main\data\zipcode-districts.json");
	ZipcodeDistricts = JsonConvert.DeserializeObject<List<ZipcodeDistrict>>(jsonZips);
	
	// Debugging.  This was null in one scenario.
	(ZipcodeDistricts == null).Dump();

	// Tested a few zip codes
 	SearchByZipCode("72956").Dump();
	
	// A good smoke-test would have been to run a forelse() on the whole zipcode collection
	// to check if any had zero or an unreasonably high number.

}

public List<Representative> Representatives;
public List<ZipcodeDistrict> ZipcodeDistricts;

// You can define other methods, fields, classes and namespaces here

public List<Representative> SearchByZipCode(string zipCode)
{

	List<Representative> result = new List<Representative>();
	
	var zipDists = ZipcodeDistricts.Where(zd => zd.zcta == zipCode).ToList();
	
	// a List<int> of district #s
	var districts = zipDists.Select(d => d.cd ).ToList();	
	
	result.AddRange(
		Representatives.Where(r => r.currentTerm.state == zipDists[0].state_abbr && districts.Contains(r.currentTerm.district) )
	);

// Toying with dynamic objects, rather than send whole Representative objects

//	var result = new {
//		cspan = ""
//	}; 
//	result.cspan = ""
	
	return result;
	
}

object List<Representative>()
{
	throw new NotImplementedException();
}

public class Representative
{
	public Id id { get; set; }
	public Name name { get; set; }
	public Bio bio { get; set; }
	public Term[] terms { get; set; }
	public Term currentTerm {
		
		get 
		{
			return this.terms.Where(t => t.end > new DateTime(2019, 1, 1)).FirstOrDefault();
		}
		
	}
}

// (Used Visual Studio's "paste-JSON-as-Classes" trick for these.)

// Started to comment out fields/properties from the JSON not needed in the 
// final output.  Newtonsoft happily ignores JSON data that doesn't map to an
// explicit field, so it's easy to trim these down without issue.
public class Id
{
	public string bioguide { get; set; }
	public string thomas { get; set; }
	public string lis { get; set; }
	//public int govtrack { get; set; }
	public string opensecrets { get; set; }
	//public int votesmart { get; set; }
	public string[] fec { get; set; }
	//public int cspan { get; set; }
	public string wikipedia { get; set; }
	//public int house_history { get; set; }
	public string ballotpedia { get; set; }
	//public int maplight { get; set; }
	public int icpsr { get; set; }
	public string wikidata { get; set; }
	public string google_entity_id { get; set; }
}

public class Name
{
	public string first { get; set; }
	public string last { get; set; }
	public string official_full { get; set; }
}

public class Bio
{
	public string birthday { get; set; }
	public string gender { get; set; }
	public string religion { get; set; }
}

public class Term
{
	public string type { get; set; }
	public DateTime start { get; set; }
	public DateTime end { get; set; }
	public string state { get; set; }
	public int district { get; set; }
	public string party { get; set; }
	public int _class { get; set; }
	public string url { get; set; }
	public string address { get; set; }
	public string phone { get; set; }
	public string fax { get; set; }
	public string contact_form { get; set; }
	public string office { get; set; }
	public string state_rank { get; set; }
	public string rss_url { get; set; }
}



public class ZipcodeDistrict
{

	public string state_fips { get; set; }
	public string state_abbr { get; set; }
	public string zcta { get; set; }
	public int cd { get; set; }

}
