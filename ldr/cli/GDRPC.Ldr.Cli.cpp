#include "pch.h"

extern "C" {
	//start gdrpc
	__declspec(dllexport) void Start() {
		GDRPC::AppRunner::Run();
	}

	//stop gdrpc (+ remove temp folder)
	__declspec(dllexport) void Stop() {
		GDRPC::AppRunner::Stop();
	}
}