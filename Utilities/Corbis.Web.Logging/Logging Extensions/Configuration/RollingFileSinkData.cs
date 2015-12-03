using System.Xml.Serialization;
using System.Configuration;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings for a RollingFileSink.
	/// </summary>
	public class RollingFileSinkData : ConfigurationElement
	{
		private string _baseFilename = "app.log";
		private string _timestampFormat = "yyyy-MM-dd [hh-mm t]";
		private AgeThresholdUnit _ageUnit = AgeThresholdUnit.Weeks;
		private int _ageThreshold = 0;
		private ByteThresholdUnit _byteUnit = ByteThresholdUnit.Kilobytes;
		private int _byteThreshold = 250;
		private string _header = "--------------------------------------------------------";
		private string _footer = string.Empty;
		private int _maxNumberOfLogs = 0;

		public RollingFileSinkData() {}

	
		/// <summary>
		/// Numeric value representing the file age threshold.  Combines with AgeUnit property.
		/// </summary>
		[XmlAttribute("ageThreshold")]
		public int AgeThreshold
		{
			get { return _ageThreshold; }
			set { _ageThreshold = value; }
		}

		/// <summary>
		/// Describes value of AgeThreshold property in Minutes, Hours, Days, Weeks or Months.
		/// </summary>
		[XmlAttribute("ageUnit")]
		public AgeThresholdUnit AgeUnit
		{
			get { return _ageUnit; }
			set { _ageUnit = value; }
		}

		/// <summary>
		/// Numeric value representing the file size threshold.  Combines with ByteUnit property.
		/// </summary>
		[XmlAttribute("byteThreshold")]
		public int ByteThreshold
		{
			get { return _byteThreshold; }
			set { _byteThreshold = value; }
		}

		/// <summary>
		/// Describes value of ByteThreshold property in Kilobytes, Megabytes or Gigabytes.
		/// </summary>
		[XmlAttribute("byteUnit")]
		public ByteThresholdUnit ByteUnit
		{
			get { return _byteUnit; }
			set { _byteUnit = value; }
		}

		/// <summary>
		/// Date-time format string.  
		/// If this value is blank, a unique incremental counter will be appended instead.
		/// If this base filename and timestamp do not generate a unique combination, 
		/// the counter will be appended.
		/// </summary>
		[XmlAttribute("timestampFormat")]
		public string TimestampFormat
		{
			get { return _timestampFormat; }
			set { _timestampFormat = value; }
		}

		/// <summary>
		/// Baes filename for the log text file.  All current log entries are written to this file.
		/// When this file fills up, it is renamed and a new, empty file is created.
		/// </summary>
		[XmlAttribute("baseFilename")]
		public string BaseFilename
		{
			get { return _baseFilename; }
			set { _baseFilename = value; }
		}

		/// <summary>
		/// Header to write before each log entry.  Optional.
		/// </summary>
		[XmlAttribute("header")]
		public string Header
		{
			get { return _header; }
			set { _header = value; }
		}

		/// <summary>
		/// Footer to write after each log entry.  Optional.
		/// </summary>
		[XmlAttribute("footer")]
		public string Footer
		{
			get { return _footer; }
			set { _footer = value; }
		}

		[XmlAttribute("maxNumberOfLogs")]
		public int MaximumLogFilesBeforePurge
		{
			get { return _maxNumberOfLogs; }
			set { _maxNumberOfLogs = value; }
		}
	}
}
