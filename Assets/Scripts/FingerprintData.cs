using System;
using System.Collections.Generic;
using System.Text;

namespace CabSvr.Fingerprint.Dtos
{
	public enum ImpressionType
	{
		LiveScanPlain = 0,
		LiveScanRolled = 1,
		NonLiveScanPlain = 2,
		NonLiveScanRolled = 3,
		Latent = 4,
		Swipe = 5,
		LiveScanContactless = 6
	}

	public static class Formats
	{
		public enum Fid
		{
			ANSI = 1770497,
			ISO = 16842759
		}
		public enum Fmd
		{
			DP_PRE_REGISTRATION = 0,
			DP_REGISTRATION = 1,
			DP_VERIFICATION = 2,
			ANSI = 1769473,
			ISO = 16842753
		}
	}

	public enum ResultCode
	{
		Success = 0,
		NotImplemented = 96075786,
		Failure = 96075787,
		NoData = 96075788,
		MoreData = 96075789,
		InvalidParameter = 96075796,
		InvalidDevice = 96075797,
		DeviceBusy = 96075806,
		DeviceFailure = 96075807,
		InvalidFID = 96075877,
		TooSmallArea = 96075878,
		InvalidFMD = 96075977,
		EnrollmentInProgress = 96076077,
		EnrollmentNotStarted = 96076078,
		EnrollmentNotReady = 96076079,
		EnrollmentInvalidSet = 96076080,
		VersionIncompatibility = 96076777
	}

	public enum CaptureQuality
	{
		Good = 0,
		TimedOut = 1,
		Cancelled = 2,
		NoFinger = 4,
		FakeFinger = 8,
		FingerTooLeft = 16,
		FingerTooRight = 32,
		FingerTooHigh = 64,
		FingerTooLow = 128,
		FingerOffCenter = 256,
		ScanSkewed = 512,
		ScanTooShort = 1024,
		ScanTooLong = 2048,
		ScanTooSlow = 4096,
		ScanTooFast = 8192,
		ScanWrongDirection = 16384,
		ReaderDirty = 32768,
		ReaderFailed = 65536
	}

	public class FingerprintData
	{
		public string Data { get; set; }
		public string UserId { get; set; }
	}

	public class FingerprintResult
	{
		// Needs to be stored
		public FingerprintData Data { get; set; }

		// Information about the scan which can be discarded

		public ResultCode Code;
		public CaptureQuality Quality { get; set; }
		public int Score { get; set; }
		public Reader Reader { get; set; }
		public byte[] RawImage { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
