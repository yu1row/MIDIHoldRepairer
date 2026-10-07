/**
 * md-to-pdf configuration for README manuals.
 * @see https://github.com/simonhaenisch/md-to-pdf
 */
module.exports = {
  stylesheet: ["docs/pdf-style.css"],
  body_class: ["manual"],
  page_media_type: "print",
  launch_options: {
    ...(process.env.PUPPETEER_EXECUTABLE_PATH
      ? { executablePath: process.env.PUPPETEER_EXECUTABLE_PATH }
      : {}),
    args: ["--no-sandbox", "--disable-dev-shm-usage"],
  },
  pdf_options: {
    format: "A4",
    margin: {
      top: "14mm",
      right: "12mm",
      bottom: "16mm",
      left: "12mm",
    },
    printBackground: true,
    displayHeaderFooter: true,
    headerTemplate: "<div></div>",
    footerTemplate:
      '<div style="width:100%;font-size:8px;color:#656d76;padding:0 12mm;display:flex;justify-content:space-between;">' +
      "<span>MIDI Hold Repairer</span>" +
      '<span><span class="pageNumber"></span> / <span class="totalPages"></span></span>' +
      "</div>",
  },
  stylesheet_encoding: "utf-8",
};
