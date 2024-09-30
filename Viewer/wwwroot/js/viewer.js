// var viewer;
// var options = {
//     env: 'AutodeskProduction2',
//     api: 'streamingV2_EU',  // for models uploaded to EMEA change this option to 'streamingV2_EU'
//     getAccessToken: function(onTokenReady) {
//         var token = 'YOUR_ACCESS_TOKEN';
//         var timeInSeconds = 3600; // Use value provided by APS Authentication (OAuth) API
//         onTokenReady(token, timeInSeconds);
//     }
// };
//
// Autodesk.Viewing.Initializer(options, function() {
//
//     var htmlDiv = document.getElementById('forgeViewer');
//     viewer = new Autodesk.Viewing.GuiViewer3D(htmlDiv);
//     var startedCode = viewer.start();
//     if (startedCode > 0) {
//         console.error('Failed to create a Viewer: WebGL not supported.');
//         return;
//     }
//
//     console.log('Initialization complete, loading a model next...');
//
// });

var viewer;

async function launchViewer(urn, accessToken) {
    var options = {
        env: "AutodeskProduction2",
        api: 'streamingV2_EU',
        getAccessToken: (callback) => callback(accessToken, 3600),
    };

    console.log('lalalala');
    Autodesk.Viewing.Initializer(options, () => {
        var documentId = "urn:" + urn;
        Autodesk.Viewing.Document.load(
            documentId,
            onDocumentLoadSuccess,
            onDocumentLoadFailure
        );
    });
}

function onDocumentLoadSuccess(doc) {
    var viewables = doc.getRoot().getDefaultGeometry();
    viewer = new Autodesk.Viewing.GuiViewer3D(
        document.getElementById("forgeViewer"),
        { extensions: ["Autodesk.DocumentBrowser"] }
    );
    viewer.start();
    viewer.loadDocumentNode(doc, viewables).then((i) => {
        // documented loaded, any action?
    });
}

function onDocumentLoadFailure(viewerErrorCode, viewerErrorMsg) {
    console.error(
        "onDocumentLoadFailure() - errorCode:" +
        viewerErrorCode +
        "\n- errorMessage:" +
        viewerErrorMsg
    );
}