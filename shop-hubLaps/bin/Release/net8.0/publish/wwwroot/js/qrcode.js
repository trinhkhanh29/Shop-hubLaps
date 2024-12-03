window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    console.log(uri);
    if (uri) {
        $("#qrCode").empty();
        $("#qrCode").qrcode({
            text: uri,
            width: 256,
            height: 256,
        });
    } else {
        console.error("URI không hợp lệ");
    }
});