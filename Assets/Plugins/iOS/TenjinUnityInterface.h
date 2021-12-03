//
//  TenjinUnityInterface.h
//  Unity-iOS bridge
//
//  Copyright (c) 2018 Tenjin. All rights reserved.
//
//

#ifndef __Unity_iPhone__TenjinUnityInterface__
#define __Unity_iPhone__TenjinUnityInterface__

#include "TenjinSDK.h"

extern "C" {

typedef struct TenjinStringStringKeyValuePair {
    const char* key;
    const char* value;
} TenjinStringStringKeyValuePair;

typedef void (*TenjinDeeplinkHandlerFunc)(TenjinStringStringKeyValuePair* deepLinkDataPairArray, int32_t deepLinkDataPairCount);

void iosTenjinInit(const char* apiKey);
void iosTenjinInitWithSharedSecret(const char* apiKey, const char* sharedSecret);
void iosTenjinInitWithAppSubversion(const char* apiKey, int subversion);
void iosTenjinInitWithSharedSecretAppSubversion(const char* apiKey, const char* sharedSecret, int subversion);

void iosTenjinInitialize(const char* apiKey);
void iosTenjinInitializeWithSharedSecret(const char* apiKey, const char* sharedSecret);
void iosTenjinInitializeWithAppSubversion(const char* apiKey, int subversion);
void iosTenjinInitializeWithSharedSecretAppSubversion(const char* apiKey, const char* sharedSecret, int subversion);

void iosTenjinConnect();
void iosTenjinConnectWithDeferredDeeplink(const char* deferredDeeplink);

void iosTenjinSendEvent(const char* eventName);
void iosTenjinSendEventWithValue(const char* eventName, const char* eventValue);
void iosTenjinTransaction(const char* productId, const char* currencyCode, int quantity, double price);
void iosTenjinTransactionWithReceiptData(const char* productId, const char* currencyCode, int quantity, double price, const char* transactionId, const char* receipt);
void iosTenjinRegisterDeepLinkHandler(TenjinDeeplinkHandlerFunc deeplinkHandlerFunc);

void iosTenjinOptIn();
void iosTenjinOptOut();
void iosTenjinOptInParams(char** params, int size);
void iosTenjinOptOutParams(char** params, int size);
void iosTenjinAppendAppSubversion(int subversion);
void iosTenjinUpdateConversionValue(int conversionValue);
void iosTenjinRequestTrackingAuthorizationWithCompletionHandler();

void iosTenjinSetDebugLogs();
    
void iosTenjinSubscribeMoPubImpressions();
void iosTenjinMopubImpressionFromJSON(const char* jsonString);
}

#endif
