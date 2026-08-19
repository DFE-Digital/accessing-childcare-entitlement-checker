import fs from 'fs';
import path from 'path';

const srcDir = path.resolve('node_modules/govuk-frontend/dist/govuk');
const destDir = path.resolve('assets');

console.log('Copying GOV.UK Frontend assets from:', srcDir);
console.log('Target destination directory:', destDir);

// Ensure destDir exists
fs.mkdirSync(destDir, { recursive: true });

// Copy CSS
fs.mkdirSync(path.join(destDir, 'css'), { recursive: true });
fs.copyFileSync(
  path.join(srcDir, 'govuk-frontend.min.css'),
  path.join(destDir, 'css/govuk-frontend.min.css')
);

// Copy JS
fs.mkdirSync(path.join(destDir, 'js'), { recursive: true });
fs.copyFileSync(
  path.join(srcDir, 'govuk-frontend.min.js'),
  path.join(destDir, 'js/govuk-frontend.min.js')
);

// Copy assets recursively (images and fonts)
const copyDir = (src, dest) => {
  fs.mkdirSync(dest, { recursive: true });
  const entries = fs.readdirSync(src, { withFileTypes: true });
  for (let entry of entries) {
    const srcPath = path.join(src, entry.name);
    const destPath = path.join(dest, entry.name);
    if (entry.isDirectory()) {
      copyDir(srcPath, destPath);
    } else {
      fs.copyFileSync(srcPath, destPath);
    }
  }
};

copyDir(path.join(srcDir, 'assets'), destDir);
console.log('GOV.UK Frontend assets copied successfully!');
