using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommonScreenResolutions
{
	gameConsole_16bit_pocket_10x9, //160 	× 	144 	160∶144 	10:9 	1∶1
	gameConsole_16bit_low_4x3, //256 	× 	224 	256∶224 	4∶3 	7∶6
	gameConsole_16bit_super_4x3, //400 	× 	300 	4∶3 	4∶3 	1∶1
	gameConsole_32bit_low_4x3, //256 	× 	224 	256∶224 	4∶3 	7∶6 
	gameConsole_32bit_standard_4x3, //320 	× 	240 	4∶3 
	gameConsole_32bit_VGA_4x3, //640 	× 	480 	4∶3 	4∶3 	1∶1
	TV_NTSC_4x3, //~440 × 486 	4:3 	
	TV_PAL_4x3, // 	~520 × 576 	4:3 	~299,520
	TV_PAL_wide_16x9, //	1024 	× 	576 	16∶9 	16∶9 	1∶1
	TV_720_4x3, //960 × 720 	4:3
	TV_720_wide_16x9, //1280 × 720 16:9 
	TV_FullHD_16x9, //1920 	× 	1080 	16∶9 	16∶9
	TV_4K_16x9, //3840 	× 	2160 	16∶9 	16∶9 	1∶1
	TV_8k_16x9, //7680 	× 	4320 	16∶9 	16∶9 	1∶1
	BETA, //~380 × 486
	VHS_NTSC, //320 × 486
	VHS_S_PAL, //420 × 576 
	DVD_NTSC_4x3, //720 × 480 
	DVD_PAL_4x3, //720 × 576 
	DVD_NTSC_16x9, //720 × 1.77777 = 405 
	DVD_PAL_16x9 //720 × 1.77777 = 405 
}

public static class ScreenResolutionUtility
{
	public static void SetScreenResolution(CommonScreenResolutions Resolution, FullScreenMode fullScreenMode, uint refreshRateHz)
	{
		var refreshRate = new RefreshRate();
		refreshRate.numerator = refreshRateHz;
		refreshRate.denominator = 1;
		switch(Resolution)
		{
		case CommonScreenResolutions.gameConsole_16bit_pocket_10x9:
			Screen.SetResolution(160, 144, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.gameConsole_16bit_low_4x3:
			Screen.SetResolution(256, 224, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.gameConsole_16bit_super_4x3:
			Screen.SetResolution(400, 300, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.gameConsole_32bit_low_4x3:
			Screen.SetResolution(256, 224, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.gameConsole_32bit_standard_4x3:
			Screen.SetResolution(320, 240, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.gameConsole_32bit_VGA_4x3:
			Screen.SetResolution(640, 480, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_NTSC_4x3:
			Screen.SetResolution(440, 486, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_PAL_4x3:
			Screen.SetResolution(520, 576, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_PAL_wide_16x9:
			Screen.SetResolution(1024, 576, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_720_4x3:
			Screen.SetResolution(960, 720, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_720_wide_16x9:
			Screen.SetResolution(1280, 720, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_FullHD_16x9:
			Screen.SetResolution(1920, 1080, fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_4K_16x9:
			Screen.SetResolution(3840, 2160,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.TV_8k_16x9:
			Screen.SetResolution(7680, 4320,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.BETA:
			Screen.SetResolution(380, 486,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.VHS_NTSC:
			Screen.SetResolution(320, 486,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.VHS_S_PAL:
			Screen.SetResolution(420, 576,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.DVD_NTSC_4x3:
			Screen.SetResolution(720, 480,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.DVD_PAL_4x3:
			Screen.SetResolution(720, 576,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.DVD_NTSC_16x9:
			Screen.SetResolution(720, 405,  fullScreenMode,refreshRate);
			break;
		case CommonScreenResolutions.DVD_PAL_16x9:
			Screen.SetResolution(720, 405,  fullScreenMode,refreshRate);
			break;
		}
	}
}
