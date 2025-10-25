document.getElementById("addVariant").addEventListener("click", function () {
    var table = document.getElementById("variantsTable").getElementsByTagName('tbody')[0];
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    row.innerHTML = `
                <td><input name="Product.Variants[${rowCount}].VariantName" class="form-control" /></td>
                <td><input name="Product.Variants[${rowCount}].Sku" class="form-control" /></td>
                <td><input name="Product.Variants[${rowCount}].Stock" type="number" class="form-control" /></td>
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
    const previewContainer = document.getElementById("previewContainer");
    const imageInput = document.getElementById("imageInput");
    const form = document.querySelector("form");

    // đọc danh sách ảnh cũ từ data attribute (một nguồn duy nhất)
    let existingImages = [];
    document.querySelectorAll('input[name="ExistingImages"]').forEach(input => { existingImages.push({ url: input.value, isDeleted: false }); });

    let newImages = [];
    renderPreview();
    // ====== 2️⃣ Khi chọn ảnh mới ======
    imageInput.addEventListener("change", () => {
        const files = Array.from(imageInput.files);

        files.forEach(file => {
            if (!file.type.startsWith("image/")) return;

            const reader = new FileReader();
            reader.onload = (e) => {
                // Không thêm trùng
                if (!newImages.some(x => x.url === e.target.result)) {
                    newImages.push({
                        name: file.name,
                        url: e.target.result,
                        file: file
                    });
                    renderPreview();
                }
            };
            reader.readAsDataURL(file);
        });

        imageInput.value = "";
    });

    // ====== 3️⃣ Render preview ảnh cũ + ảnh mới ======
    function renderPreview() {
        previewContainer.innerHTML = "";

        // --- Ảnh cũ ---
        existingImages.forEach((img, index) => {
            if (img.isDeleted) return; // không render ảnh bị xóa

            const wrapper = document.createElement("div");
            wrapper.classList.add("position-relative", "m-2");
            wrapper.style.width = "120px";

            wrapper.innerHTML = `
                <img src="${img.url}" alt="old_${index}"
                     class="img-thumbnail shadow-sm"
                     style="width: 100%; height: 100px; object-fit: cover; border-radius: 6px;" />
                <button type="button"
                        class="btn btn-sm btn-danger position-absolute top-0 end-0 translate-middle"
                        style="border-radius: 50%; width: 25px; height: 25px;"
                        data-type="old" data-index="${index}">
                    ×
                </button>
                <div class="small text-muted mt-1 text-center">Ảnh cũ #${index + 1}</div>
            `;

            previewContainer.appendChild(wrapper);
        });

        // --- Ảnh mới ---
        newImages.forEach((img, index) => {
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
                        data-type="new" data-index="${index}">
                    ×
                </button>
                <div class="small text-muted mt-1 text-center">Ảnh mới #${index + 1}</div>
            `;

            previewContainer.appendChild(wrapper);
        });

        bindDeleteEvents();
    }

    // ====== 4️⃣ Xử lý xóa ảnh ======
    function bindDeleteEvents() {
        previewContainer.querySelectorAll(".btn-danger").forEach(btn => {
            btn.addEventListener("click", e => {
                const type = e.currentTarget.getAttribute("data-type");
                const index = parseInt(e.currentTarget.getAttribute("data-index"));

                if (type === "old") {
                    existingImages[index].isDeleted = true; // đánh dấu xóa ảnh cũ
                } else if (type === "new") {
                    newImages.splice(index, 1); // xóa ảnh mới
                }

                renderPreview();
            });
        });
    }

    // ====== 5️⃣ Trước khi submit ======
    if (form) {
        form.addEventListener("submit", e => {
            // Xóa tất cả input ExistingImages cũ (nếu có, phòng ngừa)
            // đảm bảo xóa toàn bộ trước khi DataTransfer thêm vào
            form.querySelectorAll('input[name="ExistingImages"]').forEach(x => x.remove());

            // thêm đoạn log kiểm tra
            console.log("Đã xóa input cũ, còn lại:", form.querySelectorAll('input[name="ExistingImages"]').length);
            form.querySelectorAll(".hidden-upload").forEach(x => x.remove());

            // Gắn file mới (DataTransfer)
            const dt = new DataTransfer();
            newImages.forEach(img => dt.items.add(img.file));

            const hiddenFileInput = document.createElement("input");
            hiddenFileInput.type = "file";
            hiddenFileInput.name = "ImageFiles";
            hiddenFileInput.multiple = true;
            hiddenFileInput.files = dt.files;
            hiddenFileInput.classList.add("hidden-upload");
            form.appendChild(hiddenFileInput);

            // Gắn lại danh sách các ảnh cũ còn giữ
            existingImages
                .filter(img => !img.isDeleted)
                .forEach(img => {
                    const hiddenInput = document.createElement("input");
                    hiddenInput.type = "hidden";
                    hiddenInput.name = "ExistingImages";
                    hiddenInput.value = img.url;
                    hiddenInput.classList.add("hidden-upload");
                    form.appendChild(hiddenInput);
                });
        });
    }

    renderPreview();

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