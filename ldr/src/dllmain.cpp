// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <iostream>
#include <windows.h>

typedef void* (__cdecl* fRun)();
fRun run;
HMODULE rpc;
DWORD WINAPI Ldr(void* hMod) {
    rpc = LoadLibrary(L"gdrpc_cli.dll");
    run = (fRun)GetProcAddress(rpc, "Start");
    run();
    return 0;
}

BOOL WINAPI DllMain(HMODULE hMod, DWORD reason, LPVOID reserved) {
    if (reason == DLL_PROCESS_ATTACH) {
        CreateThread(0, 0x1000, Ldr, hMod, 0, 0);
    }
    return true;
}
