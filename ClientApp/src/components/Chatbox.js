import { ArrowForwardIcon } from "@chakra-ui/icons";
import {
  Box,
  Flex,
  Input,
  Button,
  InputLeftElement,
  InputRightElement,
  InputGroup,
} from "@chakra-ui/react";

import React, { useEffect, useRef, useState } from "react";
import ScrollList from "./ScrollList";

export default function Chatbox({ file }) {
  const [message, setMessage] = useState([]);
  const [messageIndex,setMessageIndex]=useState(0);
  const [messageLoading, setMessageLoading] = useState(false);
  const inputRef = useRef(null);
  const handleClick = async () => {
    setMessageLoading(true);
    const prompt = inputRef.current.value;

    var data = { text: prompt, role: "USER" };
    setMessage((prevMessages) => [...prevMessages, data]);
    try {
      const res = await fetch("https://localhost:7126/api/PDF/AskQuestion", {
        method: "POST",
        body: JSON.stringify(data),
        mode: "cors",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const answer = await res.json();
      setMessageLoading(false);

      setMessage((prevMessages) => [...prevMessages, answer]);
    } catch (error) {
      console.log(error);
      setMessageLoading(false);
    }
  };
  return (
    <Flex
      minHeight="100vh"
      w="100%"
      justify="center"
      flexDirection="column"
      align="center"
    >
      <ScrollList messageList={message} />
      <InputGroup w="70%" alignContent="flex-end">
        <Input
          ref={inputRef}
          id="message"
          type="text"
          name="message"
          disabled={messageLoading}
          h="5vh"
          placeholder="Send a Message"
        />
        <InputRightElement>
          <Button type="submit" onClick={handleClick}>
            <ArrowForwardIcon color="gray.300" fontSize="2xl" />
          </Button>
        </InputRightElement>
      </InputGroup>
    </Flex>
  );
}
