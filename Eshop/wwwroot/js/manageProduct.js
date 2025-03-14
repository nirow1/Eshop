window.addEventListener("load", () => {
    const images = [...document.querySelectorAll("#product-images-administration a")];

    for (const image of images)
        image.addEventListener("click", async e => await removeImage(image, images));
});

async function removeImage(image, images) {
    if (confirm("Opravdu si přejete odstranit vybraný náhled")) {
        const productId = document.querySelector("#Product_ProductId").value;
        const imageIndex = image.getAttribute("data-image-index");

        try {
            await fetch(`/Product/DeleteImage?productID=${productId}&imageIndex=${imageIndex}`, { method: "POST" });
        }
        catch {
            alert("Odebrání se nezdařilo");
            return;
        }

        const nextImages = images.slice(images.indexOf(image) + 1, images.lenght);
        for (const nextImage of nextImages) {
            const newIndex = parseInt(nextImage.getAttribute("data-image-index")) - 1;
            nextImage.setAttribute("data-image-index", newIndex);
        }

        image.parentElement.remove();

        const imagesCountEl = document.querySelector("#Product_ImagesCount");
        const imagesCount = parseInt(imagesCountEl.value);

        if (!isNan(imagesCount) && imagesCount > 0) {
            imagesCountEl.value = imagesCount - 1;
        }
    }
}

