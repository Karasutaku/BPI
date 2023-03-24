export async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

//export async function downloadFileFromStream(fileName, contentStreamReference) {
//    const arrayBuffer = await contentStreamReference.arrayBuffer();

//    const blob = new Blob([arrayBuffer], { type: 'video/mp4' });
//    const url = URL.createObjectURL(blob);
//    window.open(url);

//    //const anchorElement = document.createElement('a');
//    //anchorElement.href = url;
//    //anchorElement.download = fileName ?? '';
//    //anchorElement.click();
//    //anchorElement.remove();

//    URL.revokeObjectURL(url);
//}

export function showAlert(message) {
    alert(message);
}