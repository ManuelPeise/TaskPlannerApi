import React from "react";
import PageLayout from "./Lib/Layout/PageLayout";
import { BrowserRouter, Routes, Route } from "react-router-dom";
interface IProps {}

const MainPage: React.FC<IProps> = () => {
  return (
    <PageLayout>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<div>MainPage</div>} />
        </Routes>
      </BrowserRouter>
    </PageLayout>
  );
};

export default MainPage;
