import React from "react";
import './HomepageLayout.css'; // optional separate CSS

export const HomepageLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="homepageWrapper">
      {children}
    </div>
  );
};
