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
    const form = document.querySelector("form");
    let imagesData = [];

    if (!imageInput || !previewContainer) return;

    // ====== Khi chọn ảnh ======
    imageInput.addEventListener("change", () => {
        const files = Array.from(imageInput.files);

        // Giữ lại ảnh cũ, chỉ thêm mới vào
        files.forEach(file => {
            if (!file.type.startsWith("image/")) return;

            const reader = new FileReader();
            reader.onload = (e) => {
                // Không thêm trùng
                if (!imagesData.some(x => x.url === e.target.result)) {
                    imagesData.push({
                        name: file.name,
                        url: e.target.result,
                        file: file
                    });
                    renderPreview();
                }
            };
            reader.readAsDataURL(file);
        });

        // reset input để chọn lại cùng ảnh nếu cần
        imageInput.value = "";
    });

    // ====== Render preview ======
    function renderPreview() {
        previewContainer.innerHTML = "";

        imagesData.forEach((img, index) => {
            const wrapper = document.createElement("div");
            wrapper.classList.add("position-relative", "m-2");
            wrapper.style.width = "120px";

            wrapper.innerHTML = `
                <img src="${img.url}" alt="${img.name}"
                    class="img-thumbnail shadow-sm"
                    style="width: 100%; height: 100px; object-fit: cover; border-radius: 6px;" />

                <button type="button"
                        class="btn btn-sm btn-danger position-absolute top-0 end-0 translate-middle"
                        style="border-radius: 50%; width: 25px; height: 25px;"
                        data-index="${index}">
                    ×
                </button>
                <div class="small text-muted mt-1 text-center">#${index + 1}</div>
            `;

            previewContainer.appendChild(wrapper);
        });

        bindDeleteEvents();
    }

    // ====== Xử lý xóa ảnh ======
    function bindDeleteEvents() {
        previewContainer.querySelectorAll(".btn-danger").forEach(btn => {
            btn.addEventListener("click", e => {
                const idx = parseInt(e.currentTarget.getAttribute("data-index"));
                imagesData.splice(idx, 1);
                renderPreview();
            });
        });
    }

    // ====== Khi submit form ======
    if (form) {
        form.addEventListener("submit", e => {
            // Xóa input ẩn cũ
            form.querySelectorAll(".hidden-upload").forEach(x => x.remove());

            const dt = new DataTransfer();
            imagesData.forEach(img => dt.items.add(img.file));

            const hiddenFileInput = document.createElement("input");
            hiddenFileInput.type = "file";
            hiddenFileInput.name = "ImageFiles";
            hiddenFileInput.multiple = true;
            hiddenFileInput.files = dt.files;
            hiddenFileInput.classList.add("hidden-upload");

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