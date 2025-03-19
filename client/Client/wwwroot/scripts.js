window.downloadFileFromStream = async (fileName, streamReference, type) => {
    const arrayBuffer = await streamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer], { type: type });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
};
