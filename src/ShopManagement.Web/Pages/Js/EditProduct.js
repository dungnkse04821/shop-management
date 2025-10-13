document.getElementById("addVariant").addEventListener("click", function () {
    var table = document.getElementById("variantsTable").getElementsByTagName('tbody')[0];
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    row.innerHTML = `
                <td><input name="ViewModel.Product.Variants[${rowCount}].VariantName" class="form-control" /></td>
                <td><input name="ViewModel.Product.Variants[${rowCount}].Sku" class="form-control" /></td>
                <td><input name="ViewModel.Product.Variants[${rowCount}].Stock" type="number" class="form-control" /></td>
                <td><button type="button" class="btn btn-danger remove-variant">X</button></td>
            `;
});

document.addEventListener("click", function (e) {
    if (e.target && e.target.classList.contains("remove-variant")) {
        e.target.closest("tr").remove();
    }
});


document.addEventListener("click", function (e) {
    if (e.target && e.target.classList.contains("remove-image")) {
        e.target.closest("tr").remove();
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const imageInput = document.getElementById("imageInput");
    const previewContainer = document.getElementById("previewContainer");
    let imagesData = [];

    if (!imageInput || !previewContainer) return;

    // ====== Khi chọn ảnh ======
    imageInput.addEventListener("change", () => {
        const files = Array.from(imageInput.files);

        files.forEach((file) => {
            if (!file.type.startsWith("image/")) return;

            const reader = new FileReader();
            reader.onload = (e) => {
                imagesData.push({
                    name: file.name,
                    url: e.target.result,
                    file: file
                });
                renderPreview();
            };
            reader.readAsDataURL(file);
        });

        //imageInput.value = "";
    });

    // ====== Hàm render preview ======
    function renderPreview() {
        previewContainer.innerHTML = "";

        imagesData.forEach((img, index) => {
            const wrapper = document.createElement("div");
            wrapper.classList.add("m-2", "text-center", "position-relative", "draggable-item");
            wrapper.style.width = "120px";
            wrapper.setAttribute("draggable", "true");
            wrapper.setAttribute("data-index", index);

            wrapper.innerHTML = `
                <img src="${img.url}"
                     alt="preview"
                     class="preview-img-thumbnail shadow-sm"
                     style="width: 100%; height: 100px; object-fit: cover; border-radius: 8px;" />

                <button type="button"
                        class="btn btn-sm btn-danger position-absolute top-0 end-0 translate-middle"
                        style="border-radius: 50%; width: 25px; height: 25px; line-height: 1;"
                        data-index="${index}">
                    ×
                </button>

                <div class="small text-muted mt-1">#${index + 1}</div>

                <input type="hidden" name="Product.Images[${index}].SortOrder" value="${index + 1}" />
                <input type="hidden" name="Product.Images[${index}].FileName" value="${img.name}" />
            `;

            previewContainer.appendChild(wrapper);
        });

        addDeleteEvents();
        addDragDropEvents();
    }

    // ====== Xử lý xóa ảnh ======
    function addDeleteEvents() {
        previewContainer.querySelectorAll(".btn-danger").forEach(btn => {
            btn.addEventListener("click", (e) => {
                const index = parseInt(e.currentTarget.getAttribute("data-index"));
                imagesData.splice(index, 1);
                renderPreview();
            });
        });
    }

    // ====== Xử lý Drag & Drop ======
    function addDragDropEvents() {
        const draggables = previewContainer.querySelectorAll(".draggable-item");

        draggables.forEach((item) => {
            item.addEventListener("dragstart", handleDragStart);
            item.addEventListener("dragover", handleDragOver);
            item.addEventListener("drop", handleDrop);
            item.addEventListener("dragend", handleDragEnd);
        });
    }

    let draggedItem = null;

    function handleDragStart(e) {
        draggedItem = this;
        this.classList.add("dragging");
        e.dataTransfer.effectAllowed = "move";
    }

    function handleDragOver(e) {
        e.preventDefault();
        const draggingOverItem = this;

        if (draggingOverItem === draggedItem) return;

        const allItems = Array.from(previewContainer.children);
        const draggingIndex = allItems.indexOf(draggedItem);
        const overIndex = allItems.indexOf(draggingOverItem);

        if (draggingIndex < overIndex) {
            previewContainer.insertBefore(draggedItem, draggingOverItem.nextSibling);
        } else {
            previewContainer.insertBefore(draggedItem, draggingOverItem);
        }
    }

    function handleDrop(e) {
        e.stopPropagation();
        updateImagesOrder();
    }

    function handleDragEnd(e) {
        this.classList.remove("dragging");
        updateImagesOrder();
    }

    // ====== Cập nhật lại thứ tự ảnh (SortOrder) sau khi kéo thả ======
    function updateImagesOrder() {
        const newOrder = Array.from(previewContainer.children).map(div => {
            const imgTag = div.querySelector("img");
            const fileName = imgTag?.getAttribute("alt") || "";
            const found = imagesData.find(i => i.name === fileName);
            return found || null;
        }).filter(Boolean);

        // Nếu không tìm thấy theo alt, fallback theo url
        if (newOrder.length !== imagesData.length) {
            const urls = Array.from(previewContainer.querySelectorAll("img")).map(img => img.src);
            imagesData.sort((a, b) => urls.indexOf(a.url) - urls.indexOf(b.url));
        } else {
            imagesData = newOrder;
        }

        renderPreview();
    }

    // ====== 🔥 Gắn file thực vào form khi submit ======
    const form = document.querySelector("form");
    if (form) {
        form.addEventListener("submit", (e) => {
            // Xóa các input ẩn cũ (nếu có)
            form.querySelectorAll(".hidden-upload").forEach(x => x.remove());

            // Tạo đối tượng DataTransfer để gom file
            const dt = new DataTransfer();
            imagesData.forEach(img => dt.items.add(img.file));

            // Tạo input file ẩn mang toàn bộ file user đã chọn
            const hiddenFileInput = document.createElement("input");
            hiddenFileInput.type = "file";
            hiddenFileInput.name = "ImageFiles";
            hiddenFileInput.multiple = true;
            hiddenFileInput.files = dt.files;
            hiddenFileInput.classList.add("hidden-upload");

            // Thêm vào form
            form.appendChild(hiddenFileInput);
        });
    }
});



document.addEventListener("DOMContentLoaded", function () {
    const thumbnails = document.querySelectorAll(".thumbnail-img");
    const carousel = document.querySelector("#productImageCarousel");

    // Đồng bộ thumbnail khi click
    thumbnails.forEach(tn => {
        tn.addEventListener("click", function () {
            thumbnails.forEach(t => t.classList.remove("active"));
            this.classList.add("active");
        });
    });

    if (carousel !== null) {
        // Khi carousel thay đổi slide, đổi thumbnail active
        carousel.addEventListener('slide.bs.carousel', function (e) {
            thumbnails.forEach(t => t.classList.remove("active"));
            thumbnails[e.to].classList.add("active");
        });
    }

    // Đặt ảnh đầu tiên là active
    if (thumbnails.length > 0) {
        thumbnails[0].classList.add("active");
    }
});