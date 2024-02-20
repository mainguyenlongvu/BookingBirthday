(function (global) {
  'use strict';

  const nonce = window['dfBootstrapNonce'];

  let policy = {createScriptURL: url => url};
  if (global.trustedTypes) {
    policy = global.trustedTypes.createPolicy('dialogflow#messenger', policy);
  }

  var CUSTOM_ELEMENTS_ADAPTER_URL =
      'https://static.dialogflow.com/common/messenger/webcomponentsjs/2.1.3/custom-elements-es5-adapter.js';
  var WEBCOMPONENTS_LOADER_URL =
      'https://static.dialogflow.com/common/messenger/webcomponentsjs/2.1.3/webcomponents-loader.js';
  var MESSENGER_URL =
      'https://www.gstatic.com/dialogflow-console/fast/messenger/messenger-internal.min.js?v=4';

  var loadDfMessenger = function () {
    var elementScript = document.createElement('script');
    elementScript.addEventListener('load', onMessengerLoaded, false);
    elementScript.src = policy.createScriptURL(MESSENGER_URL);
    if (nonce) {
      elementScript.setAttribute('nonce', nonce);
    }
    global.document.body.insertBefore(elementScript, null);
  };

  var onMessengerLoaded = function () {
    window.dispatchEvent(new Event('dfMessengerLoaded'))
  };

  var loadWebComponentPolyfills = function () {
    var customElementsAdapterTag = document.createElement('script');
    if (global.customElements) {
      // Import custom elements adapter which is needed for Custom element
      // classes transpiled to ES5.
      customElementsAdapterTag.src = policy.createScriptURL(CUSTOM_ELEMENTS_ADAPTER_URL);
      if (nonce) {
        customElementsAdapterTag.setAttribute('nonce', nonce);
      }
      document.head.appendChild(customElementsAdapterTag);
    }
    // Import web components loader which loads polyfills based on browser
    // support.
    const webComponentsLoaderTag = document.createElement('script');
    webComponentsLoaderTag.src = policy.createScriptURL(WEBCOMPONENTS_LOADER_URL);
    if (nonce) {
      webComponentsLoaderTag.setAttribute('nonce', nonce);
    }
    global.document.head.appendChild(webComponentsLoaderTag);
  };

  global.addEventListener('WebComponentsReady', loadDfMessenger, false);

  var raf = global.requestAnimationFrame || global.mozRequestAnimationFrame ||
      global.webkitRequestAnimationFrame || global.msRequestAnimationFrame;
  if (raf) {
    raf(function () {
      global.setTimeout(loadWebComponentPolyfills, 0);
    });
  } else {
    global.addEventListener('load', loadWebComponentPolyfills);
  }
})(window);

