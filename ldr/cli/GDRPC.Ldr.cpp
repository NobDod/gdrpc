#include "pch.h"

#include "GDRPC.Ldr.h"
extern "C" __declspec(dllexport) void Start() {
	GDRPC::AppRunner::Run();
}